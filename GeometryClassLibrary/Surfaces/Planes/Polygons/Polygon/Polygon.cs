using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnitClassLibrary;

namespace GeometryClassLibrary
{
    /// <summary>
    /// A plane region is a section of a plane.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public partial class Polygon : Plane
    {
        public static explicit operator Polygon(PlaneRegion p)
        {
            return new Polygon(p.Edges.Select(e => (LineSegment)e).ToList(), false);
        }

        #region Properties and Fields

        [JsonProperty]
        public virtual List<LineSegment> LineSegments { get; set; }

        public virtual List<Point> Vertices { get { return LineSegments.GetAllPoints(); } }
        
        /// <summary>
        /// determines if the polygon is convex
        /// i.e. all segments whose endpoints are inside the polygon, are inside the polygon
        /// </summary>
        public bool IsConvex
        {
            get
            {
                if (_isConvex == null)
                {
                    _isConvex = _getIsConvex();
                }
                return (bool)_isConvex;
            }
        }
        private bool? _isConvex = null;

        private bool _getIsConvex()
        {
            int n = LineSegments.Count;
            Vector crossProduct;
            for (int i = 0; i < n; i++)
            {
                if (i < n - 1)
                {
                    crossProduct = this.LineSegments[i].CrossProduct(LineSegments[i + 1]);
                }
                else
                {
                    crossProduct = this.LineSegments.Last().CrossProduct(LineSegments.First());
                }
                if (crossProduct.Magnitude != Distance.Zero && crossProduct.Direction != NormalVector.Direction)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// finds the average of the vertices.
        ///Not the same as centroid, but close enough for smallish convex polygons
        ///is the same for every triangle
        ///faster algorithm running time than centroid
        /// </summary>
        public Point CenterPoint
        {
            get
            {
                if (_centerPoint == null)
                {
                    _centerPoint = this.Vertices.CenterPoint();
                }
                return _centerPoint;
            }
        }
        private Point _centerPoint;

        /// <summary>
        /// _findArea returns a possibly negative area to make the centroid formula work right
        /// the Area property takes absolute value before returning
        /// </summary>
        public virtual Area Area
        {
            get
            {   
                if (_area == null)
                {
                    _area = new Area(AreaType.InchesSquared, Math.Abs(_findArea()));
                }
                return _area;
            }
        }
        private Area _area;

        /// <summary>
        /// Returns the centroid (geometric center point) of the Polygon
        /// i.e. the center of mass for a plate of uniform density
        /// slower than CenterPoint algorithm
        /// </summary>
        /// <returns>the region's center as a point</returns>
        public virtual Point Centroid
        {
            get
            {
                if (_centroid == null)
                {
                    _centroid = _findCentroid();
                }
                return _centroid;
            }
        }
        private Point _centroid;

        #endregion

        #region Constructors

        /// <summary>
        /// Null constructor
        /// </summary>
        protected Polygon() { }

        /// <summary>
        /// Makes a polygon by connecting the points with line segments in the order they are in the list. If they are not in the correct order you can tell it
        /// to sort the linessegments of the polygon clockwise with the boolean flag unless you need it in the specific order it is in
        /// </summary>
        /// <param name="passedPoints">The List of points to make the polygon with. It will create the linesegments based on the order the points are inputted</param>
        public Polygon(List<Point> passedPoints, bool shouldValidate = true)
            : this(passedPoints.MakeIntoLineSegmentsThatMeet(), shouldValidate)
        { }

        /// <summary>
        /// Creates a polygon from the passed linesegments, after validating that they in fact form a closed nondegenerate planar region.
        /// </summary>
        [JsonConstructor]
        public Polygon(List<LineSegment> lineSegments, bool shouldValidate = true)
        {
            if (shouldValidate)
            {
                this.LineSegments = (lineSegments.FixSegmentOrientation());
            }
            else
            {
                this.LineSegments = (lineSegments).ToList();
            }

            this.BasePoint = lineSegments[0].EndPoint;

            this.NormalVector = this._getUnitNormalVector();
        }

        private Vector _getUnitNormalVector()
        {
            Vector vector1 = (LineSegments.OrderBy(s => s.BasePoint.X).ThenBy(s => s.BasePoint.Y).ThenBy(s => s.BasePoint.Z).First());

            Vector vector2;
            Vector normal = null;
            for (int i = 0; i < LineSegments.Count; i++)
            {
                vector2 = new Vector(vector1.BasePoint, Vertices[i]);
                normal = vector1.CrossProduct(vector2);
                if (normal.Magnitude != Distance.Zero)
                {
                    break;
                }
            }

            return new Vector(this.BasePoint, normal.Direction, Distance.Inch);
        }
        /// <summary>
        /// Defines a plane region using the given lines and where they intersect as long as the lines are all coplanar
        /// ToDo: Needs a unit test
        /// </summary>
        /// <param name="passedBoundaries"></param>
        public Polygon(List<Line> passedLines)
            : this(_generateLineSegmentsFromIntersectingLines(passedLines)) { }

        private static List<LineSegment> _generateLineSegmentsFromIntersectingLines(List<Line> passedLines)
        {
            List<LineSegment> toUse = new List<LineSegment>();

            //find where they each intersect
            for (int i = 0; i < passedLines.Count; i++)
            {
                List<Point> intersections = new List<Point>();

                for (int j = i + 1; j < passedLines.Count; j++)
                {
                    Point intersection = passedLines[i].IntersectWithLine(passedLines[j]);
                    if (intersection != null && !intersections.Contains(intersection))
                    {
                        intersections.Add(intersection);
                    }

                    if (intersections.Count == 2)
                    {
                        toUse.Add(new LineSegment(intersections[0], intersections[1]));
                    }
                    else
                    {
                        throw new ArgumentException("lines are invalid");
                    }
                }
            }
            
            return toUse;
        }

        /// <summary>
        /// Creates a new Polygon that is a copy of the passed polygon
        /// </summary>
        /// <param name="passedBoundaries"></param>
        public Polygon(Polygon polygonToCopy)
            : base(polygonToCopy)
        {
            this.LineSegments = polygonToCopy.LineSegments.ToList();
        }

        #endregion

        #region Overloaded Operators

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Not a perfect equality operator, is only accurate up to the Distance Class's accuracy
        /// </summary>
        public static bool operator ==(Polygon region1, Polygon region2)
        {
            if ((object)region1 == null)
            {
                if ((object)region2 == null)
                {
                    return true;
                }
                return false;
            }
            return region1.Equals(region2);
        }

        /// <summary>
        /// Not a perfect equality operator, is only accurate up to the Distance Class's accuracy
        /// </summary>
        public static bool operator !=(Polygon region1, Polygon region2)
        {
            if ((object)region1 == null)
            {
                if ((object)region2 == null)
                {
                    return false;
                }
                return true;
            }
            return !region1.Equals(region2);
        }

        /// <summary>
        /// does the same thing as ==
        /// </summary>
        public override bool Equals(object obj)
        {
            //make sure we didnt get a null
            if (obj == null || !(obj is Polygon))
            {
                return false;
            }
            Polygon comparablePolygon = (Polygon)obj;

            //if they have differnt number of boundaries they cant be equal
            if (this.LineSegments.Count != comparablePolygon.LineSegments.Count)
            {
                return false;
            }

            //now check each line segment
            foreach (LineSegment segment in this.LineSegments)
            {
                //make sure each segment is represented exactly once
                int timesUsed = 0;
                foreach (LineSegment segmentOther in comparablePolygon.LineSegments)
                {
                    if (segment == segmentOther)
                    {
                        timesUsed++;
                    }
                }

                //make sure each is used exactly once
                if (timesUsed != 1)
                {
                    return false;
                }
            }

            //if the segments were all the same, then they're equal
            return true;
        }

        

        #endregion

        #region Methods

        public new Polygon Shift(Shift passedShift)
        {
            return new Polygon(this.LineSegments.Shift(passedShift), false);
        }

        /// <summary>
        /// Rotates the polygon about the given axis by the specified angle
        /// </summary>
        public new Polygon Rotate(Rotation rotationToApply)
        {
            List<LineSegment> newBoundaryList = new List<LineSegment>();
            foreach (LineSegment segment in this.LineSegments)
            {
                newBoundaryList.Add(segment.Rotate(rotationToApply));
            }

            return new Polygon(newBoundaryList, false);
        }


        public new Polygon Translate(Translation translation)
        {
            return new Polygon(this.Vertices.Select(v => v.Translate(translation)).ToList(), false);
        }

        public virtual Polygon SmallestEnclosingRectangle()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <example>
        /// 
        ///                                                                                                                             
        ///                                                                                                                  
        ///                               ~~==============================================================================: 
        ///                               MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM7 
        ///                               MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMI 
        ///                               MMMM,                                                                       $MMMI 
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       $MMMI     
        ///                               MMMM:                                                                       ZMMMI     
        ///                               MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMI     
        ///                               MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMI     
        ///                               O8OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO?     
        ///                                                                                                                     
        ///                                                                                                                     
        ///                                                                +DMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM$    
        ///                                                               ~OMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMO     
        ///                                                              :ZMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM,     
        ///                                                             ,ZMMMD+                                     $MMMM,      
        ///                                                             ?DMMM$,                                    ,MMMM$       
        ///                                                            ?DMMM7                                     ,NMMM$        
        ///                                                           ~8MMMZ:                                     OMMM8,        
        ///                                                          ,OMMMO~                                     $MMMM,         
        ///                                                          $NMMD?                                     :MMMM?          
        ///                                                         7NMMN?                                     :NMMM7           
        ///                                                        ~8MMM$~                                     7MMMN,           
        ///                                                       :DMMMZ:                                     $MMMM:            
        ///                                                      ,DMMMZ,                                     OMMMM,             
        ///                                                      $MMMM=,                                    :MMMM$              
        ///                                                     7MMMM+                                     :NMMMZ               
        ///                                                    =NMMM$,                                     ZMMMN                
        ///                                                   =MMMM7,                                     ZMMMD                 
        ///                                                   DNMMN~                                     ~MMMMI                 
        ///                                                  8MMMN:                                     ~MMMM?                  
        ///                                                 +MMMMI,                                     OMMMN                   
        ///                                                +MMMM+                                      OMMMN                    
        ///                                               ,NMMMZ                                      +MMMM=                    
        ///                                               8MMMD                                      :MMMM?                     
        ///                                              IMMMN~                                      8MMM8                      
        ///                                             +MMMM=                                      OMMMN,                      
        ///                                             DMMM8                                      =MMMM?                       
        ///                                            DMMM8                                      +MMMMI                        
        ///                                           8MMMD                                     ,=MMMMI                         
        ///                                          =MMMM+                                      8MMMD                          
        ///                                         +MMMM+                                      8MMMM                           
        ///                                         NMMM8                                      +MMMM+                           
        ///                                        NMMMD                                      =MMMM+                            
        ///                                       IMMMM=                                      DMMMO                             
        ///                                      ?MMMM=                                      8MMM8                              
        ///                                      NMMMO                                      =MMMM+                              
        ///                                     NMMMO                                      +MMMM=                               
        ///                                    ZMMMD,                                     :NMMM$                                
        ///                                   IMMMM:                                      8MMM8                                 
        ///                                  ~NMMM7                                      IMMMN:                                 
        ///                                  NMMMZ                                      +MMMM+                                  
        ///                                 NMMMO                                      ?MMMM+                                   
        ///                                ?MMMM~                                     ,NMMM8                                    
        ///                               IMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMNMMM8                                     
        ///                               NMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM=                                     
        ///                              NMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM+                                      
        ///                                                                                                                     
        ///                                                                                                                     
        ///                                                                                                                     
        ///                                                                    :7O=                                        =Z7  
        ///                                                                 ,+OMMM$                                     ,INMMN  
        ///                                                              =OMMN$~                                     =MMMD+     
        ///                                                               7O+,                                        D$=       
        ///                                                     ?8MN=                                       =ZMN:               
        ///                                                ?I7ONMMMMZ7777777777777777777777777777777777777ONMMM8~               
        ///                                               $MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMN+                    
        ///                                              ?MMMMO$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$OMMMM8                      
        ///                                             ,NMMMO                                      ?MMMM~                      
        ///                                             8MMMD,                                     +MMMM?                       
        ///                                            $MMMM:                                     ,MMMMO                        
        ///                                           7MMMM,                                     ,MMMMZ                         
        ///                                          ,NMMM7                                      $MMMM:                         
        ///                                         ,NMMM$                                      7MMMM~                          
        ///                                         $MMMM,                                     ,MMMMZ                           
        ///                                        ZMMMM,                                     ,MMMMO                            
        ///                                       =MMMMI                                      ZMMMM:                            
        ///                                      ~MMMM$                                      7MMMM,                             
        ///                                      OMMMD:                                     :MMMM$                              
        ///                                     OMMMM,                                     ,MMMM7                               
        ///                                    ~MMMM7                                      7MMMM,                               
        ///                                   ~MMMM$                                      $MMMM,                                
        ///                                  :NMMM$                                     ,IMMMM:                                 
        ///                                  ZMMMM:                                     ,MMMM$                                  
        ///                                 7MMMM~                                      NMMMZ                                   
        ///                                ~MMMM7                                      ZMMMM,                                   
        ///                               ~MMMMI                                      ZMMMN,                                    
        ///                               ZMMMN                                      =MMMM7                                     
        ///                              ZMMMN                                      =MMMM$                                      
        ///                             =MMMMI                                     ,OMMMM,                                      
        ///                            =MMMMI                                     ,OMMMM,                                       
        ///                            DMMMO                                     ,IMMMM?                                        
        ///                           DMMMN                                     ,+MMMM7                                         
        ///                          +MMMM?                                     ~8MMMD                                          
        ///                         =MMMM?                                     ~8MMM8                                           
        ///                         8MMMM                                     ,?MMMN+                                           
        ///                        8MMMM                                     ,IMMMN+                                            
        ///                       8MMMN,                                    ,IMMMN?                                             
        ///                      +MMMMI                                     +DMMMZ                                              
        ///                     ~MMMM$             ~$8:                    =8MMMO,                                              
        ///                     8MMMN           ~$NMMM?                   :7MMM8=                                               
        ///                    DMMMD          ZMMZ=                      :ZMMM8=                                                
        ///                   +MMMM+          :I:                        INMMM$                                                 
        ///                  IMMMM+ +8MMD                               IMMMM$          ~$MMMD                                  
        ///                  NMMMD$NMMMZ+                              ~OMMM8=       ,?DMMMD?                                   
        ///                 MMMMDMZ=                                  ~8MMMO~        ,8?,                                       
        ///                $MMMN=                                    ,ZNMMNI~+7?                                                
        ///               IMMMMN88OOOOOO88OOOOOOOOOO88OOOOOO8888OO88ODMMMMMMMMNI,                                               
        ///              :NMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMNN87~                                                  
        ///              NMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMZ:                                                      
        ///                                                                                                                     
        ///                                                                                                                     
        ///                                                                    ,+I????????????????????????????????????????????I=
        ///                                                                    ?MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM~
        ///                                                                   =8MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMI 
        ///                                                            ~$DZ~ =OMMMD=~~~~~~~~~~~~~~~~~~~~~~~~~~~~~=~I8MZ==MMMMZ  
        ///                                                       =ZNMMMZ=  =8MMM8,                           ~$MMMN$: ,NMMMZ   
        ///                                                       INMO?,    7MMMD?                            =MM8?    7MMMM:   
        ///                                               ,$7             ,7MMMDI                     ,7Z             7MMMM:    
        ///                                             ?8MMN             =8MMMO                    =OMMM~           ,MMMMZ     
        ///                                        IMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMN$:             ,MMMM7      
        ///                                        NMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMZ                $MMMM,      
        ///                                       NMMM8=~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~=7MMMMI                $MMMM,       
        ///                                      ?MMMM+                                      8MMMD                ,MMMM$        
        ///                                     IMMMM+                                      DMMM8                ,MMMM7         
        ///                                    =MMMM7                                      ZMMMN:                8MMMO          
        ///                                    NMMMD                                      IMMMM=                $MMMM,          
        ///                                   ZMMMN:                                     ~MMMM7                =MMMM+           
        ///                                  IMMMM=                                      NMMM8                ,MMMM7            
        ///                                 ?MMMM~                                      DMMMD                ,MMMM$             
        ///                                 NMMMZ                                      IMMMM=                7MMMM:             
        ///                                NMMMO                                      IMMMM+                OMMMN,              
        ///                               IMMMM~                                     ,NMMMD                ~MMMM$               
        ///                              IMMMM:                                     :NMMMD                ~MMMM7                
        ///                             :MMMM$                                     ,7MMMM=                OMMMM,                
        ///                            ,MMMMO                                     ,IMMMM+                OMMMN                  
        ///                            7MMMM~                                     ~MMMM8                :MMMMI                  
        ///                           7MMMM:                                     ~NMMMD                ~MMMM?                   
        ///                          :NMMMZ                                     ,$MMMN=                OMMMD                    
        ///                         ,MMMMO                                     :$MMMN:                ZMMMM                     
        ///                        ,DMMM8,                                    ,7MMMN+                7MMMM~                     
        ///                        $MMMM~                                     +MMMN$                ~MMMM?                      
        ///                       +MMMM?                                     =8MMMO                ,NMMMO                       
        ///                      ,MMMM7                                     :OMMM8:                OMMMN                        
        ///                     ,NMMM$                                     =8MMMO:                ZMMMN,                        
        ///                     $MMMM,                                     7MMMN7                +MMMM?                         
        ///                    IMMMM,                                     7MMMNI                +MMMNI                          
        ///                   :MMMM$                                     =8MMMO:                DMMMN                           
        ///                   MMMM7                                     +DMMMO,               ,DMMMN                            
        ///                  $MMMM,                                    :ZMMMD?                ?MMMM?                            
        ///                 7MMMM,                                    ,OMMMD+                =MMMM=                             
        ///                :MMMM$                                     ?8MMM$,                DMMMD                              
        ///               ~MMMM7                                     ?NMMM$,                8MMM8                               
        ///              :MMMM$                                     +DMMM$,                OMMMD,                               
        ///              ZMMMM,                                     8MMM8~                =MMMM+                                
        ///             IMMMM~                                    ,OMMMMMMMMMMMMMMMMMMMMMMMMMMI                                 
        ///            :MMMM$                                     INMMMMMMMMMMMMMMMMMMMMMMMMMD                                  
        ///           :DMMMZ                                     +NMMMO7777777$777Z8MMO$7777$,                                  
        ///           ZMMMM                                     :DMMMZ~        ,?OMMMNI                                         
        ///          OMMMN                                     ,NMMMZ:       :ZMD7,                                             
        ///         :MMMMI                                     $NMMM+         :=                                                
        ///        ~MMMM?                                     IMMMM7OMMM?                                                       
        ///        ZMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM8+,                                                       
        ///       OMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMD+                                                             
        ///      :NDNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNND~                                                              
        ///                                                                                                                     
        ///                                                                                                                     
        ///                                                                +ZOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO+    
        ///                                                             =ZNMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM8     
        ///                                                        :=?ZNMMMMMMM8DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDNMMMMMMMMMMM~     
        ///                                                     ~OMMMMMNZ8MMMD?                            :7NMMMMMZ7MMMM=      
        ///                                                :$MMMMMMO=   7MMMD?                        :7DMMMMMDI   ?MMMM=       
        ///                                             ,?DMMMMMD+,    =8MMMZ,                      =OMMMMMMO~     NMMMD        
        ///                                         ?DMMMMM8?,        =DMMMO,                 ,=OMMMMMDI:         NMMM8         
        ///                                     ZDNMMMMMMN8O8888888888NMMMMD8O888888888888888DMMMMMMD?           IMMMM=         
        ///                                    8MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMI ,             INMMM+          
        ///                                   +MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM                 NMMMO           
        ///                                  +MMMMI                                     ,ZMMMM,                NMMM8            
        ///                                  DMMMD                                      ~MMMM$                ?MMMM=            
        ///                                 DMMMN                                      ~MMMM$                IMMMM:             
        ///                                +MMMM?                                      OMMMM,                NMMMZ              
        ///                               +MMMM?                                      OMMMN                 NMMMZ               
        ///                              :NMMMZ                                      7MMMM~                ZMMMD,               
        ///                              8MMMN                                     ,?MMMM?                IMMMM:                
        ///                             7MMMM=                                     =NMMMO                :MMMMI                 
        ///                            +MMMM?                                     :DMMMN                 NMMMZ                  
        ///                           =MMMM?                                     :DMMMD                 NMMMZ                   
        ///                           DMMMD                                     ,IMMMM?                $MMMM~                   
        ///                          8MMM8                                     ,7MMMN?                7MMMN~                    
        ///                         +MMMM=                                     =DMMM8,               ,MMMMO                     
        ///                        +MMMM=                                     =DMMM8                ,MMMMZ                      
        ///                        NMMM8 ,                                   :$MMMN+                $MMMM~                      
        ///                       NMMMD                                     :7MMMN+                7MMMM:                       
        ///                      ?MMMM+                                     +NMMMO                :NMMMZ                        
        ///                     IMMMM+                                     ?NMMMZ                ,NMMM$                         
        ///                    +MMMM?                                     +DMMMZ                ,DMMMO                          
        ///                    NMMMD                                     :ZMMMO~                7MMMM:                          
        ///                   OMMMN:                                    ,$MMMD+                +MMMM=                           
        ///                  ?MMMM=                                     $MMMNI                ,MMMM7                            
        ///                 ~MMMM$                                     ?NMMM$,                8MMM8                             
        ///                 NMMM8                                     +8MMMZ:                7MMMM,                             
        ///                NMMMO                                     =DMMMZ:                IMMMM,                              
        ///               ?MMMM:                                     ZMMMN?                :MMMM7                               
        ///              IMMMM~                                     OMMMN+                :MMMM7                                
        ///              NMMMO                                     +NMMM$:                OMMMM,                                
        ///             NMMMZ                                     +NMMM7:                ZMMMM,                                 
        ///            ?MMMM~                                    ,OMMMD+                =MMMM$                                  
        ///           7MMMM~                                     8MMMD=                :MMMM$                                   
        ///          ,MMMMZ                                     IMMMMI,                OMMMM,                                   
        ///          MMMMZ                                     ?MMMMD88888888888888888DMMMN                                     
        ///        ,DMMM8                                     =MMMMMMMMMMMMMMMMMMMMMMMMMMN:                                     
        ///        7MMMM:                                     DMMMMMMMMMMMMMMMMMMMMMMMMMM?                                      
        ///       +MMMM+                                     8NMMD=:::::~::+78NMMMMNOI=~:                                       
        ///      ,MMMMZ                                     7MMMM=       ?8MMMMMMZ=                                             
        ///     ,MMMMO                                     7MMMM=   ?8MMMMMN$~                                                  
        ///     7MMMM,                                    :NMMMZ,~OMMMMMM8+                                                     
        ///    7MMMMDOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOMMMMMNMMMMO=                                                          
        ///   ,MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMN?,                                                            
        ///  ,MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM$,                                                                 
        ///  </example>
        public Polyhedron Extrude(Vector directionVector)
        {
            List<Polygon> faces = new List<Polygon>();
            List<LineSegment> oppositeSegments = new List<LineSegment>();

            foreach (LineSegment segment in LineSegments)
            {
                LineSegment opposite = segment.Shift(directionVector);
                oppositeSegments.Add(opposite);

                Polygon sideFace = new Polygon(new List<Point>() { segment.BasePoint, segment.EndPoint, opposite.EndPoint, opposite.BasePoint });
                faces.Add(sideFace);
            }

            faces.Add(this);
            faces.Add(new Polygon(oppositeSegments));

            var solid = new Polyhedron(faces);
            return solid;
        }

        /// <summary>
        /// This finds and returns the Polygon where the two Polygons overlap or null if they do not 
        /// the polygons must be convex for this to work
        /// </summary>
        /// <param name="planeToBeClipped">The Polygon that will be clipped (can be either a convex or concave polygon)</param>
        /// <returns>Returns the Polygon that represents where the two Polygons overlap or null if they do not overlap
        /// or only touch</returns>
        public Polygon OverlappingPolygon(Polygon otherPolygon, bool checkIfTheyAreCoplanar = true)
        {
            //if (!this.IsConvex || !otherPolygon.IsConvex)
            //{
            //    throw new Exception("OverlappingPolygon() should not be called on NonConvex polygons");
            //}

            if (checkIfTheyAreCoplanar)
            {
                if ( new Plane(this) != new Plane(otherPolygon))
                {
                    return null;
                }
            }
            //bool sameNormal = (this.NormalVector.IsParallelTo(otherPolygon.NormalVector));
            //var vector = this.NormalVectorThrough(otherPolygon.BasePoint);
            //bool sharedPoint = (vector.Magnitude.Inches < 0.05);
            //if (sameNormal && sharedPoint)
            //{
                List<Point> newVertices = this.IntersectionPoints(otherPolygon);
                
                this._addInteriorVerticesFrom(otherPolygon, newVertices);
                otherPolygon._addInteriorVerticesFrom(this, newVertices);
                
                return newVertices.ConvexHull(true);
            //}

            //if we fail to find a valid Polygon of intersection return null
            //return null;
        }

        private void _addInteriorVerticesFrom(Polygon polygon, List<Point> newVertices)
        {
            foreach (Point vertex in polygon.Vertices)
            {
                if (this.Contains(vertex))
                {
                    _addToList(newVertices, vertex);
                }
            }
        }

        private static void _addToList(List<Point> list, Point point)
        {
            if (point != null && !list.Contains(point))
            {
                list.Add(point);
            }
        }

        /// <summary>
        /// returns a list of the points of intersection between these polygons
        /// if there are any overlapping sides, the endpoints of the shared segment are included in the list
        /// </summary>
        /// <param name="otherPolygon"></param>
        /// <returns></returns>
        public List<Point> IntersectionPoints(Polygon otherPolygon)
        {
            List<Point> newVertices = new List<Point>();
            foreach (LineSegment segment in this.LineSegments)
            {
                foreach (LineSegment otherSegment in otherPolygon.LineSegments)
                {
                    if (segment.IsParallelTo(otherSegment))
                    {
                        LineSegment overlap = segment.OverlappingSegment(otherSegment);
                        if (overlap != null)
                        {
                            _addToList(newVertices, overlap.BasePoint);
                            _addToList(newVertices, overlap.EndPoint);
                        }
                    }
                    else
                    {
                        Point intersection = segment.Intersection(otherSegment);
                        if (intersection != null)
                        {
                            _addToList(newVertices, intersection);
                        }
                    }
                }
            }
            return newVertices;
        }

        /// <summary>
        /// This function Slices a plane and returns both halves of the plane, with the larger piece returning first. If the
        /// Line is not in this Plane it will return a copy of the original Polygon in a list
        /// </summary>
        /// <param name="slicingLine">The line in the Polygon to slice at</param>
        /// <returns>returns the Polygons from slicing along the Line in descending size order or the original Plane if the line
        /// is not in the plane</returns>
        public List<Polygon> Slice(Line slicingLine)
        {
            if (slicingLine == null || !((Plane)this).Contains(slicingLine))
            {
                return new List<Polygon>() { new Polygon(this) };
            }

            //find the normal direction of the plane we will use to slice with
            Vector divisionPlaneNormal = this.NormalVector.CrossProduct(slicingLine.UnitVector(DistanceType.Inch));

            //now make it with the normal we found and the lines basepoint
            Plane divisionPlane = new Plane(divisionPlaneNormal.Direction, slicingLine.BasePoint);
            slicingLine = this.Intersection(divisionPlane);

            return this._slice(slicingLine, divisionPlane);
        }

        /// <summary>
        /// This function Slices a plane and returns both halves of the plane, with the larger piece returning first. If the
        /// Plane does not intersect this Polygon it returns a copy of the original plane region in a list
        /// </summary>
        /// <param name="slicingPlane">the plane to use to slice this Polygon where they intersect</param>
        /// <returns>returns a List of the two plane Regions that represent the slices region with the region with the larger area first</returns>
        public List<Polygon> Slice(Plane slicingPlane)
        {
            Line slicingLine = this.Intersection(slicingPlane);

            return this._slice(slicingLine, slicingPlane);
        }

        #region New slice attempt
        private List<Polygon> _slice(Line slicingLine, Plane slicingPlane)
        {

            //if it doesnt intersect then return the original
            if (slicingLine == null || slicingPlane.Equals(this as Plane))
            {
                return new List<Polygon>() { new Polygon(this) };
            }

            var intersectionPoints = this.IntersectionCoplanarPoints(slicingLine);

            if (intersectionPoints.Count < 2 || (intersectionPoints.Count == 2 && this.HasSide((LineSegment)slicingLine)))
            {
                return new List<Polygon>() { new Polygon(this) };
            }

            var segments = this.LineSegments.ToList();
            foreach (var point in intersectionPoints)
            {
                foreach (var segment in segments)
                {
                    if (segment.Contains(point) && !point.IsBaseOrEndPointOf(segment))
                    {
                        segments.Remove(segment);
                        var segment1 = new LineSegment(point, segment.BasePoint);
                        var segment2 = new LineSegment(point, segment.EndPoint);
                        segments.Add(segment1);
                        segments.Add(segment2);
                        break;
                    }
                }
            }

            var separated = _segmentsInTwoLists(segments, slicingPlane);
            var list1 = separated[0];
            var list2 = separated[1];

            for (int i = 0; i < intersectionPoints.Count / 2; i++)
            {
                var basePoint = intersectionPoints[2 * i];
                var endPoint = intersectionPoints[2 * i + 1];
                list1.Add(new LineSegment(basePoint, endPoint));
                list2.Add(new LineSegment(basePoint, endPoint));
            }
            var polygon1 = (list1).CreatePolygonIfValid();
            var polygon2 = list2.CreatePolygonIfValid();
            bool breakpoint = (polygon1 == null || polygon2 == null);
            return new List<Polygon>() { polygon1, polygon2 };
        }

        private List<List<LineSegment>> _segmentsInTwoLists(List<LineSegment> segments, Plane slicingPlane)
        {
            var referencePoint = slicingPlane.NormalVector.EndPoint;
            var list1 = new List<LineSegment>();
            var list2 = segments.ToList();
            foreach (var segment in segments)
            {
                var point1 = segment.BasePoint;
                var point2 = segment.EndPoint;
                if (slicingPlane.PointIsOnSameSideAs(point1, referencePoint) || slicingPlane.PointIsOnSameSideAs(point2, referencePoint))
                {
                    list1.Add(segment);
                    list2.Remove(segment);
                }
            }
            return new List<List<LineSegment>>() { list1, list2 };
        }


        #endregion

        #region Old Slicing methods
        ///// <summary>
        ///// An internal method that slices a planeRegion into two parts and returns them
        ///// Should be called through other methods that take only a plane or a line
        ///// and let them calculate the other part to use for better consistency
        ///// </summary>
        ///// <param name="slicingLine">The line to slice this planeRegion with (where the slicing plane and this planeRegion intersect</param>
        ///// <param name="slicingPlane">the plane to use to slice this planeRegion (corresponds to the slicing line)</param>
        ///// <returns>returns a List of the two plane Regions that represent the slices region with the region with the larger area first
        ///// or just a copy of the planeRegion in a list if it does not intersect</returns>
        //private List<Polygon> _slice(Line slicingLine, Plane slicingPlane)
        //{
        //    //make sure the line is in this plane or else it shouldnt slice
        //    if (((Plane)this).Contains(slicingLine))
        //    {
        //        //          NOTE:
        //        //index 0 is our insidePlaneRegion
        //        //index 1 is our outsidePlaneRegion

        //        //get our reference point - one that we know is on a side and not on the plane (using the normal is an easy consistent way)
        //        Point referencePoint = slicingPlane.NormalVector.EndPoint;

        //        //set up our variables we will need
        //        //create our two regions that we will modify and return
        //        List<List<LineSegment>> slicedPolygonsLines = new List<List<LineSegment>>() { this.LineSegments, this.LineSegments };

        //        //keep track of all the new lines we added so that we can connect them later on (one for each region returned)
        //        List<List<LineSegment>> newSegmentsGenerated = new List<List<LineSegment>>() { new List<LineSegment>(), new List<LineSegment>() };

        //        //keep track of the ones we just want to slice but not merge in with the generated segments
        //        List<List<LineSegment>> segmentsCut = new List<List<LineSegment>>() { new List<LineSegment>(), new List<LineSegment>() };

        //        //we have to keep track of lines to move and do it after the foreach loop because we cant change the list
        //        //while we are looping through it (immutable) (one for each region returned)
        //        List<List<LineSegment>> toRemove = new List<List<LineSegment>>() { new List<LineSegment>(), new List<LineSegment>() };

        //        //loop through each segment in the planeRegion to see if and where it needs to be sliced
        //        foreach (LineSegment line in slicedPolygonsLines[0])
        //        {
        //            //find where or if the linesegment overlaps the clipping line
        //            Point intersectPoint = line.Intersection(slicingLine);

        //            //if there is an interception than we need to clip the line
        //            if (intersectPoint != null)
        //            {
        //                //if the line does not have one of its points on the plane than we need to slice it 
        //                //if (!slicingPlane.Contains(line.EndPoint) && !slicingPlane.Contains(line.BasePoint))
        //                if (!(line.EndPoint == intersectPoint) && !(line.BasePoint == intersectPoint))
        //                {
        //                    //slice the line and project the parts for the relevent polygons
        //                    _sliceLineAndProjectForInsideAndOutsidePolygons(line, intersectPoint, slicingPlane, slicingLine, referencePoint,
        //                        slicedPolygonsLines, segmentsCut, newSegmentsGenerated, toRemove);
        //                }
        //                //if there is a point on the plane than we need to remove for one region if its on the other side
        //                else
        //                {
        //                    //we only need to project it on either the inside or outside polygon
        //                    _projectSegmentForOneSide(line, slicingPlane, slicingLine, referencePoint, newSegmentsGenerated, toRemove);
        //                }
        //            }
        //            //if it doesnt intersect at all we are either completely on the inside side or the out side so we need to find out which it is
        //            else
        //            {
        //                //if one of the points is on the outside than both of them must be at this point
        //                //so we need to project it for the inisde region and leave it for the outside one
        //                if (slicingPlane.PointIsOnSameSideAs(line.BasePoint, referencePoint) || slicingPlane.PointIsOnSameSideAs(line.EndPoint, referencePoint))
        //                {
        //                    //add it to the remove list and project it for the generated segments list
        //                    _removeAndProjectLineSegment(line, slicingLine, toRemove[1], newSegmentsGenerated[1]);
        //                }
        //                //we know that it is either on the other side or on the plane so if we check that it is
        //                //not on the plane than we know it must be on the same side as the reference point
        //                //so we need to leave it for the inside region and project it for the outside one
        //                else
        //                {
        //                    //add it to the remove list and project it for the generated segments list
        //                    _removeAndProjectLineSegment(line, slicingLine, toRemove[0], newSegmentsGenerated[0]);
        //                }
        //            }
        //        }

        //        //start by removing the segments we no longer need
        //        for (int currentRegionNumber = 0; currentRegionNumber < newSegmentsGenerated.Count; currentRegionNumber++)
        //        {
        //            //remove any segments we need to from our overlapping polygon first
        //            foreach (LineSegment lineToRemove in toRemove[currentRegionNumber])
        //            {
        //                slicedPolygonsLines[currentRegionNumber].Remove(lineToRemove);
        //            }
        //        }

        //        //now add the cut segments in
        //        for (int currentRegionNumber = 0; currentRegionNumber < segmentsCut.Count; currentRegionNumber++)
        //        {
        //            //remove any segments we need to from our overlapping polygon first
        //            foreach (LineSegment lineToAdd in segmentsCut[currentRegionNumber])
        //            {
        //                slicedPolygonsLines[currentRegionNumber].Add(lineToAdd);
        //            }
        //        }

        //        //now consolidate the generated line segments into one that spans the gap created by the line segments
        //        _consolidateGeneratedLineSegments(newSegmentsGenerated, slicedPolygonsLines);

        //        //now actually create the two Polygons so we can return them and make sure they are valid
        //        List<Polygon> createdPolygons = new List<Polygon>();
        //        var polygon1 = slicedPolygonsLines[0].CreatePolygonIfValid();
        //        var polygon2 = slicedPolygonsLines[1].CreatePolygonIfValid();
        //        if (polygon1 != null)
        //        {
        //            createdPolygons.Add(polygon1);
        //        }
        //        if (polygon2 != null)
        //        {
        //            createdPolygons.Add(polygon2);
        //        }

        //        //now return them in opposite order (largest first)
        //        createdPolygons.Sort();
        //        createdPolygons.Reverse();
        //        return createdPolygons;
        //    }
        //    //if we were not in the plane than we return a copy of the orignal plane (this)
        //    return new List<Polygon>() { new Polygon(this) };
        //}

        ///// <summary>
        ///// Slices the line segment and modifies the polygons so that they reflect they slice and finally projects the necessary part of the slice
        ///// to the newSegments for the polygons
        ///// </summary>
        //private void _sliceLineAndProjectForInsideAndOutsidePolygons(LineSegment lineToSlice, Point intersectPoint, Plane slicingPlane, Line slicingLine,
        //    Point referencePoint, List<List<LineSegment>> slicedPolygons, List<List<LineSegment>> segmentsCut, List<List<LineSegment>> newSegmentsGenerated, List<List<LineSegment>> toRemove)
        //{
        //    //we know we will always get two because we already checked and confirmed intersect
        //    List<LineSegment> lineSliced = lineToSlice.Slice(intersectPoint);

        //    //sets it so the inside lineSegment is in the first spot and the outside in the Second
        //    _findAndSortInsideAndOutsideLineSegments(lineSliced, lineToSlice, slicingPlane, referencePoint);

        //    LineSegment insidePart = lineSliced[0];
        //    LineSegment outsidePart = lineSliced[1];

        //    //now we need to deal with the projection on the lines and both halves since we need to keep both 
        //    //sections the region is split into 

        //    //Deal with the outsidePlane and outside part of the line
        //    //first project it(inside line) onto the division line(this is for the outsidePlane) 
        //    LineSegment projectedLineForOutside = insidePart.ProjectOntoLine(slicingLine);

        //    //and then add it to our new segments list if its not zero length
        //    if (projectedLineForOutside != null)
        //    {
        //        newSegmentsGenerated[1].Add(projectedLineForOutside);
        //    }

        //    //now tell it to remove the original part on the outside polygon and add the cut part
        //    toRemove[1].Add(lineToSlice);
        //    segmentsCut[1].Add(outsidePart);

        //    //Deal with the insidePlane and the inside part of the line
        //    //now do it all again for the outside line too (this is for the insidePlane)
        //    LineSegment projectedLineForInside = outsidePart.ProjectOntoLine(slicingLine);
        //    if (projectedLineForInside != null)
        //    {
        //        newSegmentsGenerated[0].Add(projectedLineForInside);
        //    }

        //    //now remove it from the inside part and add the sliced line
        //    toRemove[0].Add(lineToSlice);
        //    segmentsCut[0].Add(insidePart);
        //}

        ///// <summary>
        ///// Projects the segment and adds it to the list for the polygon that needs the projected segment
        ///// </summary>
        //private void _projectSegmentForOneSide(LineSegment lineToSlice, Plane slicingPlane, Line slicingLine,
        //    Point referencePoint, List<List<LineSegment>> newSegmentsGenerated, List<List<LineSegment>> toRemove)
        //{
        //    //if the endpoint is on the slicing plane than we can determine which side based on the basepoint
        //    if (slicingPlane.Contains(lineToSlice.EndPoint))
        //    {
        //        //if the base point is on the inside than we should cut it out of the outside
        //        if (slicingPlane.PointIsOnSameSideAs(lineToSlice.BasePoint, referencePoint))
        //        {
        //            //add it to the remove list and project it for the generated segments list
        //            _removeAndProjectLineSegment(lineToSlice, slicingLine, toRemove[1], newSegmentsGenerated[1]);
        //        }
        //        //if it wasnt on the inside than it was on the outside and we need to remove it from the inside
        //        else
        //        {
        //            //add it to the remove list and project it for the generated segments list
        //            _removeAndProjectLineSegment(lineToSlice, slicingLine, toRemove[0], newSegmentsGenerated[0]);
        //        }
        //    }
        //    //if the endpoint wasnt on the line than the basepoint was and we can determine the side with the endpoint
        //    else
        //    {
        //        //if the end point is on the inside than we should cut it out of the outside
        //        if (slicingPlane.PointIsOnSameSideAs(lineToSlice.EndPoint, referencePoint))
        //        {
        //            //add it to the remove list and project it for the generated segments list
        //            _removeAndProjectLineSegment(lineToSlice, slicingLine, toRemove[1], newSegmentsGenerated[1]);
        //        }
        //        //if it wasnt on the inside than it was on the outside and we need to remove it from the inside
        //        else
        //        {
        //            //add it to the remove list and project it for the generated segments list
        //            _removeAndProjectLineSegment(lineToSlice, slicingLine, toRemove[0], newSegmentsGenerated[0]);
        //        }
        //    }
        //}

        ///// <summary>
        ///// Finds which linesegment was on the "inside" of the slice and returns it before the one that was on the "outside" of the slice
        ///// </summary>
        ///// <param name="lineSliced">Line Segments generated from the slice</param>
        ///// <param name="lineThatWasSliced">Line that was sliced to form the sliced parts</param>
        ///// <param name="slicingPlane">Plane that was used to slice the line (To tell which is "inside")</param>
        ///// <param name="referencePoint">Reference point that is on the "inside" side of the slicingPlane</param>
        //private void _findAndSortInsideAndOutsideLineSegments(List<LineSegment> lineSliced, LineSegment lineThatWasSliced, Plane slicingPlane, Point referencePoint)
        //{
        //    //guess as what line part is the outside;
        //    LineSegment insidePart = lineSliced[1];
        //    LineSegment outsidePart = lineSliced[0];

        //    //check to see if our "guess" was right

        //    //if the base point of the original line is on the other side as the reference point then that line that 
        //    //contains it is the outside part of the line
        //    if (!slicingPlane.PointIsOnSameSideAs(lineThatWasSliced.BasePoint, referencePoint))
        //    {
        //        //if the basepoint is on the lineSegment than that is the inside part
        //        if (lineThatWasSliced.BasePoint.IsOnLineSegment(lineSliced[1]))
        //        {
        //            outsidePart = lineSliced[1];
        //            insidePart = lineSliced[0];
        //        }
        //    }
        //    //if the end point of the original line is on the other side of the reference point then that line 
        //    //that contains it is actually the outside line
        //    else if (!slicingPlane.PointIsOnSameSideAs(lineThatWasSliced.EndPoint, referencePoint))
        //    {
        //        //this time if the endpoint is on the lineSegment than that is the inside part
        //        if (lineThatWasSliced.EndPoint.IsOnLineSegment(lineSliced[1]))
        //        {
        //            outsidePart = lineSliced[1];
        //            insidePart = lineSliced[0];
        //        }
        //    }
        //    //otherwise our guess was right

        //    //now modify our list
        //    lineSliced[0] = insidePart;
        //    lineSliced[1] = outsidePart;
        //}

        ///// <summary>
        ///// Adds the line to the given toRemove list and then projects it onto the projection line and adds it to the segmentsGenerated list
        ///// </summary>
        ///// <param name="line">LineSegment to remove and project</param>
        ///// <param name="toProjectOnto">Line to Project the lineSegment onto</param>
        ///// <param name="toRemoveList">List to remove the line segment from (should be for the same polygon as newSegmentGeneratedList)</param>
        ///// <param name="newSegmentsGeneratedList">list to add the projected line too (should be for the same polygon as toRemoveList)</param>
        //private void _removeAndProjectLineSegment(LineSegment line, Line toProjectOnto, List<LineSegment> toRemoveList, List<LineSegment> newSegmentsGeneratedList)
        //{
        //    //add the projection to our new segments list for the outside region
        //    LineSegment projectedLine = line.ProjectOntoLine(toProjectOnto);
        //    if (projectedLine != null)
        //    {
        //        newSegmentsGeneratedList.Add(projectedLine);
        //    }

        //    //and now we add it to the toRemove list for the outside plane so we know to get rid of it so it 
        //    //doesnt caus us problems later on
        //    toRemoveList.Add(line);
        //}

        ///// <summary>
        ///// This combines LineSegments into as few as possible based on combining lines with a shared point into one longer one. By doing this
        ///// the generated segments should be reduced to one line and span the gap that would have been created
        ///// </summary>
        ///// <param name="newSegmentsGenerated">Line segments generated by slicing the polygon</param>
        ///// <param name="slicedPlanes">the two result polygons from the slice</param>
        //private void _consolidateGeneratedLineSegments(List<List<LineSegment>> newSegmentsGenerated, List<List<LineSegment>> slicedPlanes)
        //{
        //    //combine any of the lines that share the same point so we dont have reduntent/more segments than necessary
        //    //we can do this because we know that they are all along the same line so if they share a point they are
        //    //just an extension of the other one (they all should be exensions of the same line)
        //    for (int currentRegionNumber = 0; currentRegionNumber < newSegmentsGenerated.Count; currentRegionNumber++)
        //    {
        //        //now combine those segments
        //        for (int i = 0; i < newSegmentsGenerated[currentRegionNumber].Count; i++)
        //        {
        //            for (int j = 0; j < newSegmentsGenerated[currentRegionNumber].Count; j++)
        //            {
        //                //get our lines for this round of checks
        //                LineSegment firstLine = newSegmentsGenerated[currentRegionNumber][i];
        //                LineSegment secondLine = newSegmentsGenerated[currentRegionNumber][j];

        //                //if its not the same line (or equivalent - if it is we just ignore it for now and it will work iself out as other ones combine)
        //                if (firstLine != secondLine)
        //                {
        //                    //if two points match then combine them and add the new one to the list
        //                    //then remove the two old ones
        //                    //then we need to restart it at i = 0, j = -1 (-1 because it will increment at the end and then be back to 0) otherwise 
        //                    //we may skip some or gout out of bounds
        //                    if (firstLine.BasePoint == secondLine.BasePoint)
        //                    {
        //                        newSegmentsGenerated[currentRegionNumber].Add(new LineSegment(firstLine.EndPoint, secondLine.EndPoint));
        //                        newSegmentsGenerated[currentRegionNumber].Remove(firstLine);
        //                        newSegmentsGenerated[currentRegionNumber].Remove(secondLine);
        //                        i = 0;
        //                        j = -1;
        //                    }
        //                    else if (firstLine.BasePoint == secondLine.EndPoint)
        //                    {
        //                        newSegmentsGenerated[currentRegionNumber].Add(new LineSegment(firstLine.EndPoint, secondLine.BasePoint));
        //                        newSegmentsGenerated[currentRegionNumber].Remove(firstLine);
        //                        newSegmentsGenerated[currentRegionNumber].Remove(secondLine);
        //                        i = 0;
        //                        j = -1;
        //                    }
        //                    else if (firstLine.EndPoint == secondLine.EndPoint)
        //                    {
        //                        newSegmentsGenerated[currentRegionNumber].Add(new LineSegment(firstLine.BasePoint, secondLine.BasePoint));
        //                        newSegmentsGenerated[currentRegionNumber].Remove(firstLine);
        //                        newSegmentsGenerated[currentRegionNumber].Remove(secondLine);
        //                        i = 0;
        //                        j = -1;
        //                    }
        //                    else if (firstLine.EndPoint == secondLine.BasePoint)
        //                    {
        //                        newSegmentsGenerated[currentRegionNumber].Add(new LineSegment(firstLine.BasePoint, secondLine.EndPoint));
        //                        newSegmentsGenerated[currentRegionNumber].Remove(firstLine);
        //                        newSegmentsGenerated[currentRegionNumber].Remove(secondLine);
        //                        i = 0;
        //                        j = -1;
        //                    }
        //                }
        //            }
        //        }

        //        //now we need to add the new lineSegments to our plane region
        //        foreach (LineSegment toAdd in newSegmentsGenerated[currentRegionNumber])
        //        {
        //            slicedPlanes[currentRegionNumber].Add(toAdd);
        //        }
        //    }
        //}

        #endregion


        /// <summary>
        /// Finds the intersection between this polygon and the line
        /// </summary>
        /// <param name="passedLine">The line to see if it intersects</param>
        /// <returns>Returns the point of intersection or null if it does not intersect</returns>
        public new Point IntersectWithLine(Line passedLine)
        {
            Point intersection = ((Plane)this).IntersectWithLine(passedLine);

            if(intersection != null && this.Contains(intersection))
            {
                return intersection;
            }
            return null;
        }

        /// <summary>
        /// returns the linesegment of the intersection between the polygon and the plane
        /// should not be used unless the polygon is convex
        /// </summary>
        /// <param name="plane"></param>
        /// <returns></returns>
        public LineSegment Intersection(Plane plane)
        {
            List<Point> pointsOfIntersection = new List<Point>();
            foreach(LineSegment segment in this.LineSegments)
            {
                Point point = plane.IntersectWithSegment(segment);
                if (point != null && !pointsOfIntersection.Contains(point))
                {
                    pointsOfIntersection.Add(point);
                }
            }
            if (pointsOfIntersection.Count == 2)
            {
                return new LineSegment(pointsOfIntersection[0], pointsOfIntersection[1]);
            }
            return null;
        }
        /// <summary>
        /// Returns a list of the points that line intersects the edges of the polygon
        /// </summary>
        public List<Point> IntersectionCoplanarPoints(Line passedLine)
        {
            List<Point> intersections = new List<Point>();

            foreach (LineSegment edge in this.LineSegments)
            {
                var point = edge.IntersectWithLine(passedLine);
                if (point != null && !intersections.Contains(point))
                {
                    intersections.Add(point);
                }
            }
            return intersections;
        }

        /// <summary>
        /// Returns a list of the points where the linesegment intersects the edges of the polygon
        /// </summary>
        public List<Point> IntersectionCoplanarPoints(LineSegment linesegment)
        {
            List<Point> intersections = new List<Point>();

            foreach (LineSegment edge in this.LineSegments)
            {
                Point intersection = edge.Intersection(linesegment);
                if (intersection != null && !intersections.Contains(intersection))
                {
                    intersections.Add(intersection);
                }
               
            }
            intersections = intersections.OrderBy(point => point.DistanceTo(linesegment.BasePoint)).ToList();
            return intersections;
        }


        /// <summary>
        /// Returns a list of the lineSegments of intersection through the interior of the polygon
        /// </summary>
        public List<LineSegment> IntersectionCoplanarLineSegments(Line passedLine)
        {
            
            List<LineSegment> lineSegmentsOfIntersection = new List<LineSegment>();

            List<Point> pointsOfIntersection = IntersectionCoplanarPoints(passedLine);

            for (int i = 0; 2*i + 1 < pointsOfIntersection.Count; i++ )
            {
                LineSegment newLineSegment = new LineSegment(pointsOfIntersection[2*i], pointsOfIntersection[2*i + 1]);
                lineSegmentsOfIntersection.Add(newLineSegment);
            }
            return lineSegmentsOfIntersection;
        }

        /// <summary>
        /// Returns whether or not the polygon and line intersection, but returns false if they are coplanar
        /// </summary>
        /// <param name="passedLine"></param>
        /// <returns></returns>
        public new bool DoesIntersectNotCoplanar(Line passedLine)
        {
            var point = this.IntersectWithLine(passedLine);
            return (point != null);
            //doesn't check coplanar at all..
        }

        /// <summary>
        /// Returns whether or not the given line and polygon intersect or are coplanar and intersect on the plane
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public new bool DoesIntersect(Line line)
        {
            //if the line is on the plane
            if (new Plane(this).Contains(line))
            {
                //check if it intersects our boundaries
                foreach (LineSegment segment in this.LineSegments)
                {
                    if (line.DoesIntersect(segment))
                    {
                        return true;
                    }
                }
            }

            return DoesIntersectNotCoplanar(line);
        }

        public bool DoesIntersect(LineSegment segment)
        {
            var point = this.IntersectWithLine(segment);
            return segment.Contains(point);
        }

        /// <summary>
        /// Returns true if the point is contained within this PlaneRegion, Does not include the boundaries!
        /// </summary>
        /// <param name="passedPoint">The point to see if it is in this PlaneRegion</param>
        /// <returns>returns true if the Point is in this PlaneRegion and false if it is not</returns>
        public bool ContainsNotOnBoundary(Point passedPoint)
        {
            return (Contains(passedPoint) && !Touches(passedPoint));
        }

        /// <summary>
        /// Returns true if the point is on the PlaneRegion, including on its boundaries
        /// </summary>
        public new bool Contains(Point passedPoint)
        {
            //check if it is in our plane first
            if (((Plane)this).Contains(passedPoint))
            {
                //now check if it is in the bounds each line at a time
                foreach (LineSegment line in LineSegments)
                {
                    if (line.Contains(passedPoint))
                    {
                        return true;
                    }
                }

                AngularDistance angularDistance = Angle.Zero;
                for (int i = 0; i < this.Vertices.Count; i++)
                {
                    Point previous;
                    Point next = this.Vertices[i];
                    if (i == 0)
                    {
                        previous = this.Vertices[this.Vertices.Count - 1];
                    }
                    else
                    {
                        previous = this.Vertices[i-1];
                    }
                    var segment1 = new LineSegment(passedPoint, previous);
                    var segment2 = new LineSegment(passedPoint, next);
                    
                    var angle = new AngularDistance(segment1.AngleBetween(segment2));
                    if (segment1.CrossProduct(segment2).HasOppositeDirectionOf(this.NormalVector))
                    {
                        angle = angle.Negate();
                    }
                    angularDistance += angle;

                }
               
                if (angularDistance % new AngularDistance(AngleType.Degree, 720) == Angle.Zero)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// determines if this polygon contains the entirety of the other polygon
        /// </summary>
        public new bool Contains(Polygon polygon)
        {
            //First check all vertices
            if (!this.ContainsAll(polygon.Vertices))
            {
                return false;
            }

            //if this is convex than we're done
            if (this.IsConvex)
            {
                return true;
            }

            //if not, we have to check that none of the outside vertices are inside the interior polygon
            foreach(Point vertex in this.Vertices)
            {
                if (polygon.ContainsNotOnBoundary(vertex))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks if the point is touching the PlaneRegion (aka if it is on the boundaries of the planeRegion)
        /// </summary>
        /// <param name="passedPoint">Point to check if it is touching</param>
        /// <returns>Returns true if the point touches the PlaneRegion and false if it is not on the boundaries</returns>
        public bool Touches(Point passedPoint)
        {
            //check each of our boundaries if the point is on the LineSegment
            foreach (LineSegment line in this.LineSegments)
            {
                if (passedPoint.IsOnLineSegment(line))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// determines if the polygon contains this segment as an edge
        /// </summary>
        /// <param name="segment"></param>
        /// <returns></returns>
        public bool HasSide(LineSegment segment)
        {
            foreach(LineSegment edge in LineSegments)
            {
                if (edge == segment)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Returns whether or not the two Polygons share a common side meaning that the side is exactly the same as the other's side
        /// </summary>
        /// <param name="otherPolygon">The other polygon to see if we share an exact side with</param>
        /// <returns>Returns true if the Polugons share a side that is exactly the same</returns>
        public bool DoesShareExactSide(Polygon otherPolygon)
        {
            foreach (LineSegment segment in this.LineSegments)
            {
                foreach (LineSegment segmentOther in otherPolygon.LineSegments)
                {
                    if (segment == segmentOther)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Returns whether or not any side in this polygon contains a side of the other polygon. If this polygons side is larger but contains the other it will return true
        /// </summary>
        /// <param name="otherPolygon">The other polygon to see if we share or contain in one of our sides one of its sides</param>
        /// <returns>Returns true if any of this polygon's sides contain any of the other Polygon's sides</returns>
        public bool DoesShareOrContainSide(Polygon otherPolygon)
        {
            foreach (LineSegment segment in this.LineSegments)
            {
                foreach (LineSegment segmentOther in otherPolygon.LineSegments)
                {
                    if (segment.Contains(segmentOther))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Determines whether or not the point is on the sides of this polygon
        /// </summary>
        /// <param name="pointToCheckIfItContains">The point to see if it is on the sides of this polygon</param>
        /// <returns>Returns a bool of whether or not the point is on a side of this Polygon</returns>
        public bool DoesContainPointAlongSides(Point pointToCheckIfItContains)
        {
            foreach (var segment in LineSegments)
            {
                if (segment.Contains(pointToCheckIfItContains))
                {
                    return true;
                }
            }
            return false;
        }

        //Warning: should not be used outside of certain applications. This assumes, that you are looking at a segment through adjacent intersectionpoints.
        public bool _doesContainSegmentAlongBoundary(LineSegment linesegment)
        {
            return DoesContainPointAlongSides(linesegment.BasePoint) && DoesContainPointAlongSides(linesegment.MidPoint) && DoesContainPointAlongSides(linesegment.EndPoint);
        }

        /// <summary>
        /// checks if a linesegment is a chord. i.e. endpoints on boundary & all other points interior.
        /// </summary>
        public bool Contains(LineSegment lineSegment)
        {
            if (!this.Contains(lineSegment.BasePoint) || !this.Contains(lineSegment.EndPoint))
            {
                return false;
            }
            var intersections = this.IntersectionCoplanarPoints(lineSegment);
            
            for(int i = 1; i < intersections.Count; i++)
            {
                var tempSegment = new LineSegment(intersections[i - 1], intersections[i]);
                if (!this.Contains(tempSegment.MidPoint))
                {
                    return false;
                }
            }
            return true;

        }

    
       

        //Determines the plane which we will rotate and project onto.
        private Plane _planeWithSmallestAngleBetween()
        {
            double angleXY = this.SmallestAngleBetween(XY).Degrees;
            double angleXZ = this.SmallestAngleBetween(XZ).Degrees;
            double angleYZ = this.SmallestAngleBetween(YZ).Degrees;

            double smallest = Math.Min(Math.Min(angleXY, angleXZ), angleYZ);
            if (angleXY == smallest)
            {
                return XY;
            }
            else if (angleYZ == smallest)
            {
                return YZ;
            }
            return XZ;
        }
      
        //Returns the necessary rotation, before we project
        private Rotation _rotationOfPlaneWithSmallestAngleBetweenOntoXYPlane()
        {
            Plane plane = _planeWithSmallestAngleBetween();
            if (plane == YZ)
            {
                return new Rotation(Line.YAxis, new Angle(AngleType.Degree, 90));
            }
            else if (plane == XZ)
            {
                return new Rotation(Line.ZAxis, new Angle(AngleType.Degree, 90));
            }
            else
            {
                return new Rotation(Line.XAxis, Angle.Zero);
            }
        }

        private Polygon _projectOntoXYPlane()
        {
            List<Point> projectedVertices = new List<Point>();
            foreach(Point vertex in Vertices)
            {
                projectedVertices.Add(new Point(vertex.X, vertex.Y, Distance.Zero));
            }
            return new Polygon(projectedVertices);
        }

        //Assumes the segments are ordered and circulating in a consistent direction
        //First finds which of XY, XZ, and YZ planes makes the smallest angle with the polygon
        //then rotates so that the polygon makes the smallest angle with XY
        //then we project downard and calculate area by this formula in XY:
        //Uses this formula: http://en.wikipedia.org/wiki/Polygon#Area_and_centroid
        //then we divide by cos(angle), where thats the angle between the (rotated) polygon and XY.
        //_findArea returns a possibly negative area to make centroid formula work right
        //the Area property takes absolute value before returning
        private double _findArea()
        {
            Rotation rotation = _rotationOfPlaneWithSmallestAngleBetweenOntoXYPlane();
            Polygon rotated = this.Rotate(rotation);

            List<Point> vertices = rotated.Vertices;

            Area sum = new Area(AreaType.InchesSquared, 0);
            Point previousVertex = vertices[vertices.Count - 1];

            foreach (Point vertex in vertices)
            {
                sum += previousVertex.X * vertex.Y - vertex.X * previousVertex.Y;
                previousVertex = vertex;
            }

            Angle angle = rotated.SmallestAngleBetween(XY);

            double area = sum.InchesSquared / (2 * Math.Cos(angle.Radians));

            return area;
        }

        private Point _findCentroid()
        {
            Rotation rotation = _rotationOfPlaneWithSmallestAngleBetweenOntoXYPlane();
            Polygon rotated = this.Rotate(rotation);
            Polygon projected = rotated._projectOntoXYPlane();

            List<Point> vertices = projected.Vertices;

            Distance sumX = new Distance(DistanceType.Inch, 0);
            Distance sumY = new Distance(DistanceType.Inch, 0);
            Point previousVertex = vertices[vertices.Count - 1];

            foreach (Point vertex in vertices)
            {
                sumX += (previousVertex.X + vertex.X) * (previousVertex.X * vertex.Y - vertex.X * previousVertex.Y).InchesSquared;
                sumY += (previousVertex.Y + vertex.Y) * (previousVertex.X * vertex.Y - vertex.X * previousVertex.Y).InchesSquared;
                previousVertex = vertex;
            }
            double areaOfProjected = projected._findArea();
            Distance xComp = sumX / (6 * areaOfProjected);
            Distance yComp = sumY / (6 * areaOfProjected);

            Point centroidOfProjected = new Point(xComp, yComp);

            Line lineOfProjection = new Line(Direction.Out, centroidOfProjected);
            Point centroidRotated = ((Plane)rotated).IntersectWithLine(lineOfProjection);
            Point centroid = centroidRotated.Rotate3D(rotation.Inverse());

            return centroid;

        }

        /// <summary>
        /// Returns the polygon with every linesegment's orientation reversed
        /// and the linesegments listed in opposite order
        /// so that vertices are also listed in this new order
        /// If the polygon already circulates in a consistent direction,
        /// Then this swaps the polygon's inside face with its outside one
        /// </summary>
        public Polygon ReverseOrientation()
        {
            List<LineSegment> flippedEdges = new List<LineSegment>();
            foreach(LineSegment edge in LineSegments)
            {
                flippedEdges.Add(edge.Reverse());
            }
            flippedEdges.Reverse();
            return new Polygon(flippedEdges, false);
        }

        /// <summary>
        /// Splits the polygon into connecting triangles and returns them as a list
        /// Note: the triangles will overlap if the polygon is concave
        /// </summary>
        /// <returns></returns>
        public List<Polygon> SplitIntoTriangles()
        {
            List<Polygon> triangles = new List<Polygon>();

            for (int i = 0; i < Vertices.Count - 2; i++)
            {
                List<Point> points = new List<Point>() { Vertices[0], Vertices[i + 1], Vertices[i + 2] };
                Polygon triangle = new Polygon(points, false);
                triangles.Add(triangle);
            }
            return triangles;
        }

      
        public bool IsRectangle()
        {
            if (LineSegments.Count != 4)
            {
                return false;
            }
            for (int i = 0; i < 3; i++)//we only need to check 3 angles, because the last angle is determined by the need to close the polygon.
            {
                if (LineSegments[i].AngleBetween(LineSegments[i + 1]) != new Angle(AngleType.Degree, 90))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Returns the polygon projected onto a Plane
        /// If the the polygon is perpendicular to the plane we return null
        /// </summary>
        public Polygon ProjectOntoPlane(Plane plane)
        {
            if (!this.IsPerpendicularTo(plane))
            {
                var newVertices = new List<Point>();
                foreach (Point point in this.Vertices)
                {
                    newVertices.Add(point.ProjectOntoPlane(plane));
                }

                return new Polygon(newVertices, false);
            }
            //if the polygon is perpendicular to the plane, the projection is degenerate, we get just a linesegment;
            return null;
            
        }

        public bool ContainsAll(IList<Point> pointList)
        {
            foreach (Point point in pointList)
            {
               if (!this.Contains(point))
               {
                   return false;
               }
            }
            return true;
        }

        ///// <summary>
        ///// if the passed polygon is convex, this method returns the polygonn
        ///// if the passed polygon is concave, this method returns a list of polygons:
        ///// the first being the smallest convex polygon surrounding it, the next being the region removed from that
        ///// and the next being the region removed from that, until we have a list of convex polygons
        ///// </summary>
        ///// <returns></returns>
        //public List<Polygon> AsDifferenceOfConvexPolygons()
        //{
        //    if
        //}
        #endregion

        #region Static Factory Methods

        public static Polygon EquilateralTriangle(Distance sideLength)
        {
            return RegularPolygon(3, sideLength);
        }

        /// <summary>
        /// Creates a parrelologram. 
        /// shifts both vectors so their basepoints are the passed basepoint, and creates the parrelogram spanned by those sides.
        /// </summary>
        public static Polygon Parallelogram(Vector vector1, Vector vector2, Point basePoint = null)
        {
            if (basePoint == null)
            {
                basePoint = vector1.BasePoint;
            }
            LineSegment segment1 = new LineSegment(basePoint, vector1);
            LineSegment segment2 = new LineSegment(basePoint, vector2);
            LineSegment segment3 = new LineSegment(segment2.EndPoint, vector1);
            LineSegment segment4 = new LineSegment(segment1.EndPoint, vector2);

            return new Polygon(new List<LineSegment>() { segment1, segment2, segment3, segment4 });
        }

        public static Polygon Pentagon(Distance sideLength)
        {
            return RegularPolygon(5, sideLength);
        }

        /// <summary>
        /// Creates a regular polygon centered at the origin in the XY-plane.
        /// </summary>
        public static Polygon RegularPolygon(int numberOfSides, Distance sideLength)
        { 
            if (numberOfSides < 3)
            {
                throw new ArgumentException("A polygon must have more than 2 sides.");
            }

            AngularDistance step = Angle.Degree * 360.0 / numberOfSides;
            AngularDistance otherAngle = (Angle.Degree * 180 - step) / 2;

            //Law of Sines
            Distance length = sideLength * Math.Sin(otherAngle.Radians) / Math.Sin((step.Radians));

            Point firstPoint;
            // We want the polygon to be centered at the origin,
            // and lie "flat" from the viewers perspective
            if (numberOfSides % 4 == 0)
            {
                firstPoint = new Point(length, Distance.Zero);
                firstPoint = firstPoint.Rotate2D(step / 2);
            }
            else if (numberOfSides % 2 == 0)
            {
                firstPoint = new Point(length, Distance.Zero);
            }
            else
            {
                firstPoint = new Point(Distance.Zero, length);
            }

            List<Point> points = new List<Point>() { firstPoint };
            for (int i = 1; i < numberOfSides; i++)
            {
                points.Add(firstPoint.Rotate2D(step*i));
            }

            return new Polygon(points, false);
        }

        /// <summary>
        /// Creates a polygon with the sidelength and this angle at the origin
        /// </summary>
        public static Polygon Rhombus(Angle angle, Distance sideLength)
        {
            var vector1 = Direction.Right * sideLength;
            var vector2 = vector1.Rotate(new Rotation(angle));
            return Parallelogram(vector1, vector2);
        }

        public static Polygon Square(Distance sideLength)
        {
            return RegularPolygon(4, sideLength);
        }
     
        #endregion
    }
}
