using GeometryClassLibrary.ComparisonMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary;


namespace GeometryClassLibrary
{
    /// <summary>
    /// A plane region is a section of a plane.
    /// </summary>
    public class Polygon : PlaneRegion, IComparable<Polygon>
    {
        #region Properties and Fields

        public List<LineSegment> LineSegments
        {
            get
            {
                List<LineSegment> convertedList = new List<LineSegment>();
                foreach (var linesegment in Edges)
                {
                    convertedList.Add((LineSegment)linesegment);
                }

                return convertedList;

            }
            internal set
            {
                List<IEdge> convertedList = new List<IEdge>();
                if (value != null)
                {
                    foreach (var item in value)
                    {
                        convertedList.Add(item);
                    }
                }
               
                this.Edges = convertedList;
            }
        }

        /// <summary>
        /// Finds and returns a list of all the vertices of this polygon
        /// </summary>
        private List<Point> _vertices;
        public List<Point> Vertices
        {
            get
            {
                if (_vertices == null)
                {
                    List<Point> verticesFound = new List<Point>();

                    foreach (Point point in this.LineSegments.GetAllPoints())
                    {
                        if (!verticesFound.Contains(point))
                        {
                            verticesFound.Add(point);
                        }
                    }
                    _vertices = verticesFound;
                }
                return _vertices;
            }
            internal set
            {
                _vertices = value;
            }
        }

        private bool? _isConvex = null;

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

        private bool _getIsConvex()
        {
                for (int i = 0; i + 1 < LineSegments.Count; i++)
                {
                    Vector crossProduct = this.LineSegments[i].CrossProduct(LineSegments[i + 1]);
                    if (crossProduct.Magnitude != new Distance() && crossProduct.Direction != NormalVector.Direction)
                    {
                        return false;
                    }
                }

            return true;
        }

        //finds the average of the vertices.
        //Not the same as centroid, but close enough for smallish convex polygons
        //is the same for every triangle
        //faster algorithm running time than centroid
        private Point _centerPoint;
        public Point CenterPoint
        {
            get
            {
                if (_centerPoint == null)
                {
                    Vector sum = new Vector();
                    foreach (Point vertex in Vertices)
                    {
                        sum += new Vector(vertex);
                    }
                    _centerPoint = (sum / Vertices.Count).EndPoint;
                }
                return _centerPoint;
            }
        }

        // _findArea returns a possibly negative area to make the centroid formula work right
        // the Area property takes absolute value before returning
        private Area _area;
        public override Area Area
        {
            get
            {   
                if (_area == null)
                {
                    Area area = new Area(AreaType.InchesSquared, Math.Abs(_findArea().InchesSquared));
                    _area = area;
                }
                return _area;
            }
        }

        /// <summary>
        /// Returns the centroid (geometric center point) of the Polygon
        /// i.e. the center of mass for a plate of uniform density
        /// slower than CenterPoint algorithm
        /// </summary>
        /// <returns>the region's center as a point</returns>
        private Point _centroid;
        public override Point Centroid
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

        #endregion

        #region Constructors

        /// <summary>
        /// Zero constructor
        /// </summary>
        public Polygon()
            : base()
        {
            this.LineSegments = new List<LineSegment>();
        }

        /// <summary>
        /// Makes a polygon by connecting the points with line segments in the order they are in the list. If they are not in the correct order you can tell it
        /// to sort the linessegments of the polygon clockwise with the boolean flag unless you need it in the specific order it is in
        /// </summary>
        /// <param name="passedPoints">The List of points to make the polygon with. It will create the linesegments based on the order the points are inputted</param>
        public Polygon(List<Point> passedPoints, bool shouldSort = true)
            : this(passedPoints.MakeIntoLineSegmentsThatMeet(), shouldSort) 
        {
            
        }

        /// <summary>
        /// Defines a plane region using the given boundaries as long as the line segments form a closed region
        /// </summary>
        /// <param name="passedBoundaries"></param>
        public Polygon(List<LineSegment> passedBoundaries, bool shouldSort = true)
            : base()
        {
            bool isClosed = passedBoundaries.DoFormClosedRegion();
            bool areCoplanar = passedBoundaries.AreAllCoplanar(); 
            bool notSelfIntersecting = !passedBoundaries.AtleastOneIntersection();
            if (passedBoundaries == null)
            {
                this.LineSegments = new List<LineSegment>();
            }
            else if (isClosed && areCoplanar && notSelfIntersecting)
            {
                if (shouldSort)
                {
                    this.LineSegments = passedBoundaries.SortIntoClockWiseSegments();
                }
                else
                {
                    this.LineSegments = passedBoundaries;
                }

                this.BasePoint = LineSegments[0].BasePoint;
                
                this.Edges = new List<IEdge>(LineSegments);

                this.NormalVector = this._getUnitNormalVector();
            }
            else
            {
                throw new Exception("The linesegments you are attempting to make into a polygon are either not closed or not coplanar.");
            }
        }
        ///// <summary>
        ///// Defines a plane region using the given lines and where they intersect as long as the lines are all coplanar
        ///// NOTE: Will not work for concave polygons
        ///// </summary>
        ///// <param name="passedBoundaries"></param>
        //public Polygon(List<Line> passedLines)
        //    : base(passedLines)
        //{
        //    List<LineSegment> toUse = new List<LineSegment>();

        //    if (passedLines.AreAllCoplanar())
        //    {
        //        //find where they each intersect
        //        foreach (Line line in passedLines)
        //        {
        //            List<Point> intersections = new List<Point>();

        //            foreach (Line other in passedLines)
        //            {
        //                if (line != other)
        //                {
        //                    Point intersection = line.Intersection(other);
        //                    if (intersection != null && !intersections.Contains(intersection))
        //                    {
        //                        intersections.Add(intersection);
        //                    }
        //                }
        //            }
        //            if (intersections.Count == 2)
        //            {
        //                toUse.Add(new LineSegment(intersections.ElementAt(0), intersections.ElementAt(1)));
        //            }
        //            else
        //            {
        //                throw new ArgumentException("lines are invalid");
        //            }
        //        }
        //    }
        //    else
        //    {
        //        throw new ArgumentException("lines are not coplanar");
        //    }

        //    if (!this.LineSegments.DoFormClosedRegion())
        //    {
        //        throw new ArgumentException("generated line segments from lines are invalid");
        //    }

        //    this.LineSegments = toUse;
        //}

        /// <summary>
        /// creates a new Polygon that is a copy of the passed polygon
        /// </summary>
        /// <param name="passedBoundaries"></param>
        public Polygon(Polygon polygonToCopy)
            : this(polygonToCopy.LineSegments)
        //note: we do not need to call List<LineSegment>(newplaneToCopy.Edges) because it does this in the base case for 
        //constructing a plane fron a List<LineSegment>
        {
            
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
            if (obj == null)
            {
                return false;
            }

            //try to cast the object to a Polygon, if it fails then we know the user passed in the wrong type of object
            try
            {
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

                //if the segments were are there than its equal
                return true;
            }
            //if we didnt get a polygon than its not equal
            catch (InvalidCastException)
            {
                return false;
            }
        }

        /// <summary>
        /// returns the comparison integer of -1 if less than, 0 if equal to, and 1 if greater than the other PlaneRegion
        /// NOTE: BASED SOLELY ON AREA.  MAY WANT TO CHANGE LATER
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Polygon other)
        {
            if (this.Area.Equals(other.Area))
                return 0;
            else
                return this.Area.CompareTo(other.Area);
        }

        #endregion

        #region Methods

        public new Polygon Shift(Shift passedShift)
        {
            return new Polygon(this.LineSegments.Shift(passedShift));
        }

        public override PlaneRegion ShiftAsPlaneRegion(Shift passedShift)
        {
            return this.Shift(passedShift);
        }

        /// <summary>
        /// Rotates the polygon about the given axis by the specified angle
        /// </summary>
        /// <param name="rotationToApply">The Rotation(that stores the axis to rotate around and the angle to rotate) to apply to the point</param>
        /// <returns>Returns a new Polygon that has been rotated</returns>
        public new Polygon Rotate(Rotation rotationToApply)
        {
            List<LineSegment> newBoundaryList = new List<LineSegment>();
            foreach (LineSegment segment in this.LineSegments)
            {
                newBoundaryList.Add(segment.Rotate(rotationToApply));
            }

            return new Polygon(newBoundaryList);
        }

        /// <summary>
        /// Rotates the polygon as a generic planeRegion with the given rotation
        /// </summary>
        /// <param name="passedRotation">The rotation object that is to be applied to the Polygon</param>
        /// <returns>A new Polygon as a PlaneRegion that has been rotated</returns>
        public override PlaneRegion RotateAsPlaneRegion(Rotation passedRotation)
        {
            return this.Rotate(passedRotation);
        }

        public new Polygon Translate(Point translation)
        {
            List<LineSegment> newBoundaryList = new List<LineSegment>();
            foreach (LineSegment segment in this.LineSegments)
            {
                newBoundaryList.Add(segment.Translate(new Translation(translation)));
            }
            return new Polygon(newBoundaryList);
        }

        public override Polygon SmallestRectangleThatCanSurroundThisShape()
        {
            throw new NotImplementedException();
        }

        /// <summary>
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
        ///  
        /// </summary>
        /// <param name="directionVector">the value and direction that the polygon should be extruded</param>
        /// <returns></returns>
        public new Polyhedron Extrude(Vector directionVector)
        {

            // find two lines that are not parallel

            LineSegment firstLine = this.LineSegments[0];
            LineSegment secondLine = null;
            foreach (var lineseg in this.LineSegments)
            {
                if (!lineseg.IsParallelTo(firstLine))
                {
                    secondLine = lineseg;
                    break;
                }
            }

            if (secondLine == null)
            {
                throw new Exception("There are no LineSegments in this plane region that are not parallel");
            }

            //create back Polygon
            List<Polygon> unconstructedReturnGeometry = new List<Polygon>();
            List<LineSegment> backPolygonLines = new List<LineSegment>();
            List<LineSegment> otherPolygonLines = new List<LineSegment>();

            foreach (var linesegment in this.LineSegments)
            {
                List<LineSegment> polygonConstruct = new List<LineSegment>();

                Point newBackBasePoint = linesegment.BasePoint.Translate(new Translation(directionVector.EndPoint));
                Point newBackEndPoint = linesegment.EndPoint.Translate(new Translation(directionVector.EndPoint));
                LineSegment newBackLine = new LineSegment(newBackBasePoint, newBackEndPoint);
                backPolygonLines.Add(newBackLine);

                LineSegment newNormalLine1 = new LineSegment(newBackBasePoint, linesegment.BasePoint);
                LineSegment newNormalLine2 = new LineSegment(newBackEndPoint, linesegment.EndPoint);

                unconstructedReturnGeometry.Add(
                    new Polygon(
                        new List<LineSegment> {
                            linesegment,
                            newBackLine,
                            newNormalLine1,
                            newNormalLine2
                        }
                    )
                );
            }

            unconstructedReturnGeometry.Add(this);
            unconstructedReturnGeometry.Add(new Polygon(backPolygonLines));

            return new Polyhedron(unconstructedReturnGeometry);
        }

        /// <summary>
        /// Extrudes the plane region into a Polyhedron
        /// Note: does the same as extrude, but returns it as a polyhedron instead of a Solid
        /// </summary>
        /// <param name="Distance"></param>
        /// <returns></returns>
        public Polyhedron ExtrudeAsPolyhedron(Vector extrusionVector)
        {
            return (Polyhedron)Extrude(extrusionVector);
        }

        /// <summary>
        /// Returns true if the Polygon is valid (is a closed region and the LineSegments are all coplaner)
        /// </summary>
        /// <returns>returns true if the LineSegments form a closed area and they are all coplaner</returns>
        public bool isValidPolygon()
        {
            bool isClosed = LineSegments.DoFormClosedRegion();
            bool areCoplanar = LineSegments.AreAllCoplanar();

            if (isClosed && areCoplanar)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// This finds and returns the Polygon where the two Polygons overlap or null if they do not 
        /// the polygons must be convex for this to work
        /// </summary>
        /// <param name="planeToBeClipped">The Polygon that will be clipped (can be either a convex or concave polygon)</param>
        /// <returns>Returns the Polygon that represents where the two Polygons overlap or null if they do not overlap
        /// or only touch</returns>
        public Polygon OverlappingPolygon(Polygon otherPolygon)
        {
            if (!this.IsConvex || !otherPolygon.IsConvex)
            {
                throw new Exception("OverlappingPolygon() should not be called on NonConvex polygons");
            }

            //if they are coplanar
            if (((Plane)this).Contains(otherPolygon))
            {
                List<Point> newVertices = this.IntersectionPoints(otherPolygon);
                
                this._addInteriorVerticesFrom(otherPolygon, newVertices);
                otherPolygon._addInteriorVerticesFrom(this, newVertices);
                
                return newVertices.ConvexHull(true);
            }

            //if we fail to find a valid Polygon of intersection return null
            return null;
        }

        private void _addInteriorVerticesFrom(Polygon polygon, List<Point> newVertices)
        {
            foreach (Point vertex in polygon.Vertices)
            {
                if (this.ContainsInclusive(vertex))
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
            //find the normal direction of the plane we will use to slice with
            Vector divisionPlaneNormal = this.NormalVector.UnitVector(DistanceType.Inch).CrossProduct(slicingLine.UnitVector(DistanceType.Inch));

            //now make it with the normal we found anf the lines basepoint
            Plane divisionPlane = new Plane(divisionPlaneNormal.Direction, slicingLine.BasePoint);

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

            //if it doesnt intersect then return the original
            if (slicingLine == null || (Plane)this == slicingPlane)
            {
                return new List<Polygon>() { new Polygon(this) };
            }

            return this._slice(slicingLine, slicingPlane);
        }

        /// <summary>
        /// An internal method that slices a planeRegion into two parts and returns them
        /// Should be called through other methods that take only a plane or a line
        /// and let them calculate the other part to use for better consistency
        /// </summary>
        /// <param name="slicingLine">The line to slice this planeRegion with (where the slicing plane and this planeRegion intersect</param>
        /// <param name="slicingPlane">the plane to use to slice this planeRegion (corresponds to the slicing line)</param>
        /// <returns>returns a List of the two plane Regions that represent the slices region with the region with the larger area first
        /// or just a copy of the planeRegion in a list if it does not intersect</returns>
        private List<Polygon> _slice(Line slicingLine, Plane slicingPlane)
        {
            //make sure the line is in this plane or else it shouldnt slice
            if (((Plane)this).Contains(slicingLine))
            {
                //          NOTE:
                //index 0 is our insidePlaneRegion
                //index 1 is our outsidePlaneRegion

                //get our reference point - one that we know is on a side and not on the plane (using the normal is an easy consistent way)
                Point referencePoint = slicingPlane.NormalVector.EndPoint;

                //set up our variables we will need
                //create our two regions that we will modify and return
                List<List<LineSegment>> slicedPolygonsLines = new List<List<LineSegment>>() { this.LineSegments, this.LineSegments };

                //keep track of all the new lines we added so that we can connect them later on (one for each region returned)
                List<List<LineSegment>> newSegmentsGenerated = new List<List<LineSegment>>() { new List<LineSegment>(), new List<LineSegment>() };

                //keep track of the ones we just want to slice but not merge in with the generated segments
                List<List<LineSegment>> segmentsCut = new List<List<LineSegment>>() { new List<LineSegment>(), new List<LineSegment>() };

                //we have to keep track of lines to move and do it after the foreach loop because we cant change the list
                //while we are looping through it (immutable) (one for each region returned)
                List<List<LineSegment>> toRemove = new List<List<LineSegment>>() { new List<LineSegment>(), new List<LineSegment>() };

                //loop through each segment in the planeRegion to see if and where it needs to be sliced
                foreach (LineSegment line in slicedPolygonsLines[0])
                {
                    //find where or if the linesegment overlaps the clipping line
                    Point intersectPoint = line.Intersection(slicingLine);

                    //if there is an interception than we need to clip the line
                    if (intersectPoint != null)
                    {
                        //if the line does not have one of its points on the plane than we need to slice it 
                        //if (!slicingPlane.Contains(line.EndPoint) && !slicingPlane.Contains(line.BasePoint))
                        if(!(line.EndPoint == intersectPoint) && !(line.BasePoint == intersectPoint))
                        {
                            //slice the line and project the parts for the relevent polygons
                            _sliceLineAndProjectForInsideAndOutsidePolygons(line, intersectPoint, slicingPlane, slicingLine, referencePoint,
                                slicedPolygonsLines, segmentsCut, newSegmentsGenerated, toRemove);
                        }
                        //if there is a point on the plane than we need to remove for one region if its on the other side
                        else
                        {
                            //we only need to project it on either the inside or outside polygon
                            _projectSegmentForOneSide(line, slicingPlane, slicingLine, referencePoint, newSegmentsGenerated, toRemove);
                        }
                    }
                    //if it doesnt intersect at all we are either completely on the inside side or the out side so we need to find out which it is
                    else
                    {
                        //if one of the points is on the outside than both of them must be at this point
                        //so we need to project it for the inisde region and leave it for the outside one
                        if (slicingPlane.PointIsOnSameSideAs(line.BasePoint, referencePoint) || slicingPlane.PointIsOnSameSideAs(line.EndPoint, referencePoint))
                        {
                            //add it to the remove list and project it for the generated segments list
                            _removeAndProjectLineSegment(line, slicingLine, toRemove[1], newSegmentsGenerated[1]);
                        }
                        //we know that it is either on the other side or on the plane so if we check that it is
                        //not on the plane than we know it must be on the same side as the reference point
                        //so we need to leave it for the inside region and project it for the outside one
                        else
                        {
                            //add it to the remove list and project it for the generated segments list
                            _removeAndProjectLineSegment(line, slicingLine, toRemove[0], newSegmentsGenerated[0]);
                        }
                    }
                }

                //start by removing the segments we no longer need
                for (int currentRegionNumber = 0; currentRegionNumber < newSegmentsGenerated.Count; currentRegionNumber++)
                {
                    //remove any segments we need to from our overlapping polygon first
                    foreach (LineSegment lineToRemove in toRemove[currentRegionNumber])
                    {
                        slicedPolygonsLines[currentRegionNumber].Remove(lineToRemove);
                    }
                }

                //now add the cut segments in
                for (int currentRegionNumber = 0; currentRegionNumber < segmentsCut.Count; currentRegionNumber++)
                {
                    //remove any segments we need to from our overlapping polygon first
                    foreach (LineSegment lineToAdd in segmentsCut[currentRegionNumber])
                    {
                        slicedPolygonsLines[currentRegionNumber].Add(lineToAdd);
                    }
                }

                //now consolidate the generated line segments into one that spans the gap created by the line segments
                _consolidateGeneratedLineSegments(newSegmentsGenerated, slicedPolygonsLines);

                //now actually create the two Polygons so we can return them and make sure they are valid
                List<Polygon> createdPolygons = new List<Polygon>();
                createdPolygons.Add(new Polygon(slicedPolygonsLines[0]));
                createdPolygons.Add(new Polygon(slicedPolygonsLines[1]));

                //make sure that the polygons are actually cut
                if (createdPolygons[0].LineSegments.Count <= 2 && createdPolygons[0].isValidPolygon())
                {
                    if (createdPolygons[1] == this)
                    {
                        return new List<Polygon>() { createdPolygons[1] };
                    }
                    else //shouldnt ever get here
                    {
                        throw new Exception();
                    }
                }
                else if (createdPolygons[1].LineSegments.Count <= 2 && createdPolygons[1].isValidPolygon())
                {
                    if (createdPolygons[0] == this)
                    {
                        return new List<Polygon>() { createdPolygons[0] };
                    }
                    else //shouldnt ever get here
                    {
                        throw new Exception();
                    }
                }

                //now return them in opposite order (largest first
                createdPolygons.Sort();
                createdPolygons.Reverse();
                return createdPolygons;
            }
            //if we were not in the plane than we return a copy of the orignal plane (this)
            return new List<Polygon>() { new Polygon(this) };
        }

        /// <summary>
        /// Slices the line segment and modifies the polygons so that they reflect they slice and finally projects the necessary part of the slice
        /// to the newSegments for the polygons
        /// </summary>
        /// <param name="lineToSlice"></param>
        /// <param name="intersectPoint"></param>
        /// <param name="slicingPlane"></param>
        /// <param name="slicingLine"></param>
        /// <param name="referencePoint"></param>
        /// <param name="slicedPolygons"></param>
        /// <param name="newSegmentsGenerated"></param>
        private void _sliceLineAndProjectForInsideAndOutsidePolygons(LineSegment lineToSlice, Point intersectPoint, Plane slicingPlane, Line slicingLine,
            Point referencePoint, List<List<LineSegment>> slicedPolygons, List<List<LineSegment>> segmentsCut, List<List<LineSegment>> newSegmentsGenerated, List<List<LineSegment>> toRemove)
        {
            //we know we will always get two because we already checked and confirmed intersect
            List<LineSegment> lineSliced = lineToSlice.Slice(intersectPoint);

            //sets it so the inside lineSegment is in the first spot and the outside in the Second
            _findAndSortInsideAndOutsideLineSegments(lineSliced, lineToSlice, slicingPlane, referencePoint);

            LineSegment insidePart = lineSliced[0];
            LineSegment outsidePart = lineSliced[1];

            //now we need to deal with the projection on the lines and both halves since we need to keep both 
            //sections the region is split into 

            //Deal with the outsidePlane and outside part of the line
            //first project it(inside line) onto the division line(this is for the outsidePlane) 
            LineSegment projectedLineForOutside = insidePart.ProjectOntoLine(slicingLine);

            //and then add it to our new segments list if its not zero length
            if (projectedLineForOutside != null)
            {
                newSegmentsGenerated[1].Add(projectedLineForOutside);
            }

            //now tell it to remove the original part on the outside polygon and add the cut part
            toRemove[1].Add(lineToSlice);
            segmentsCut[1].Add(outsidePart);

            //Deal with the insidePlane and the inside part of the line
            //now do it all again for the outside line too (this is for the insidePlane)
            LineSegment projectedLineForInside = outsidePart.ProjectOntoLine(slicingLine);
            if (projectedLineForInside != null)
            {
                newSegmentsGenerated[0].Add(projectedLineForInside);
            }

            //now remove it from the inside part and add the sliced line
            toRemove[0].Add(lineToSlice);
            segmentsCut[0].Add(insidePart);
        }

        /// <summary>
        /// Projects the segment and adds it to the list for the polygon that needs the projected segment
        /// </summary>
        /// <param name="lineToSlice"></param>
        /// <param name="slicingPlane"></param>
        /// <param name="slicingLine"></param>
        /// <param name="referencePoint"></param>
        /// <param name="newSegmentsGenerated"></param>
        /// <param name="toRemove"></param>
        private void _projectSegmentForOneSide(LineSegment lineToSlice, Plane slicingPlane, Line slicingLine,
            Point referencePoint, List<List<LineSegment>> newSegmentsGenerated, List<List<LineSegment>> toRemove)
        {
            //if the endpoint is on the slicing plane than we can determine which side based on the basepoint
            if (slicingPlane.Contains(lineToSlice.EndPoint))
            {
                //if the base point is on the inside than we should cut it out of the outside
                if (slicingPlane.PointIsOnSameSideAs(lineToSlice.BasePoint, referencePoint))
                {
                    //add it to the remove list and project it for the generated segments list
                    _removeAndProjectLineSegment(lineToSlice, slicingLine, toRemove[1], newSegmentsGenerated[1]);
                }
                //if it wasnt on the inside than it was on the outside and we need to remove it from the inside
                else
                {
                    //add it to the remove list and project it for the generated segments list
                    _removeAndProjectLineSegment(lineToSlice, slicingLine, toRemove[0], newSegmentsGenerated[0]);
                }
            }
            //if the endpoint wasnt on the line than the basepoint was and we can determine the side with the endpoint
            else
            {
                //if the end point is on the inside than we should cut it out of the outside
                if (slicingPlane.PointIsOnSameSideAs(lineToSlice.EndPoint, referencePoint))
                {
                    //add it to the remove list and project it for the generated segments list
                    _removeAndProjectLineSegment(lineToSlice, slicingLine, toRemove[1], newSegmentsGenerated[1]);
                }
                //if it wasnt on the inside than it was on the outside and we need to remove it from the inside
                else
                {
                    //add it to the remove list and project it for the generated segments list
                    _removeAndProjectLineSegment(lineToSlice, slicingLine, toRemove[0], newSegmentsGenerated[0]);
                }
            }
        }

        /// <summary>
        /// Finds which linesegment was on the "inside" of the slice and returns it before the one that was on the "outside" of the slice
        /// </summary>
        /// <param name="lineSliced">Line Segments generated from the slice</param>
        /// <param name="lineThatWasSliced">Line that was sliced to form the sliced parts</param>
        /// <param name="slicingPlane">Plane that was used to slice the line (To tell which is "inside")</param>
        /// <param name="referencePoint">Reference point that is on the "inside" side of the slicingPlane</param>
        private void _findAndSortInsideAndOutsideLineSegments(List<LineSegment> lineSliced, LineSegment lineThatWasSliced, Plane slicingPlane, Point referencePoint)
        {
            //guess as what line part is the outside;
            LineSegment insidePart = lineSliced[1];
            LineSegment outsidePart = lineSliced[0];

            //check to see if our "guess" was right

            //if the base point of the original line is on the other side as the reference point then that line that 
            //contains it is the outside part of the line
            if (!slicingPlane.PointIsOnSameSideAs(lineThatWasSliced.BasePoint, referencePoint))
            {
                //if the basepoint is on the lineSegment than that is the inside part
                if (lineThatWasSliced.BasePoint.IsOnLineSegment(lineSliced[1]))
                {
                    outsidePart = lineSliced[1];
                    insidePart = lineSliced[0];
                }
            }
            //if the end point of the original line is on the other side of the reference point then that line 
            //that contains it is actually the outside line
            else if (!slicingPlane.PointIsOnSameSideAs(lineThatWasSliced.EndPoint, referencePoint))
            {
                //this time if the endpoint is on the lineSegment than that is the inside part
                if (lineThatWasSliced.EndPoint.IsOnLineSegment(lineSliced[1]))
                {
                    outsidePart = lineSliced[1];
                    insidePart = lineSliced[0];
                }
            }
            //otherwise our guess was right

            //now modify our list
            lineSliced[0] = insidePart;
            lineSliced[1] = outsidePart;
        }

        /// <summary>
        /// Adds the line to the given toRemove list and then projects it onto the projection line and adds it to the segmentsGenerated list
        /// </summary>
        /// <param name="line">LineSegment to remove and project</param>
        /// <param name="toProjectOnto">Line to Project the lineSegment onto</param>
        /// <param name="toRemoveList">List to remove the line segment from (should be for the same polygon as newSegmentGeneratedList)</param>
        /// <param name="newSegmentsGeneratedList">list to add the projected line too (should be for the same polygon as toRemoveList)</param>
        private void _removeAndProjectLineSegment(LineSegment line, Line toProjectOnto, List<LineSegment> toRemoveList, List<LineSegment> newSegmentsGeneratedList)
        {
            //add the projection to our new segments list for the outside region
            LineSegment projectedLine = line.ProjectOntoLine(toProjectOnto);
            if (projectedLine != null)
            {
                newSegmentsGeneratedList.Add(projectedLine);
            }

            //and now we add it to the toRemove list for the outside plane so we know to get rid of it so it 
            //doesnt caus us problems later on
            toRemoveList.Add(line);
        }

        /// <summary>
        /// This combines LineSegments into as few as possible based on combining lines with a shared point into one longer one. By doing this
        /// the generated segments should be reduced to one line and span the gap that would have been created
        /// </summary>
        /// <param name="newSegmentsGenerated">Line segments generated by slicing the polygon</param>
        /// <param name="slicedPlanes">the two result polygons from the slice</param>
        private void _consolidateGeneratedLineSegments(List<List<LineSegment>> newSegmentsGenerated, List<List<LineSegment>> slicedPlanes)
        {
            //combine any of the lines that share the same point so we dont have reduntent/more segments than necessary
            //we can do this because we know that they are all along the same line so if they share a point they are
            //just an extension of the other one (they all should be exensions of the same line)
            for (int currentRegionNumber = 0; currentRegionNumber < newSegmentsGenerated.Count; currentRegionNumber++)
            {
                //now combine those segments
                for (int i = 0; i < newSegmentsGenerated[currentRegionNumber].Count; i++)
                {
                    for (int j = 0; j < newSegmentsGenerated[currentRegionNumber].Count; j++)
                    {
                        //get our lines for this round of checks
                        LineSegment firstLine = newSegmentsGenerated[currentRegionNumber][i];
                        LineSegment secondLine = newSegmentsGenerated[currentRegionNumber][j];

                        //if its not the same line (or equivalent - if it is we just ignore it for now and it will work iself out as other ones combine)
                        if (firstLine != secondLine)
                        {
                            //if two points match then combine them and add the new one to the list
                            //then remove the two old ones
                            //then we need to restart it at i = 0, j = -1 (-1 because it will increment at the end and then be back to 0) otherwise 
                            //we may skip some or gout out of bounds
                            if (firstLine.BasePoint == secondLine.BasePoint)
                            {
                                newSegmentsGenerated[currentRegionNumber].Add(new LineSegment(firstLine.EndPoint, secondLine.EndPoint));
                                newSegmentsGenerated[currentRegionNumber].Remove(firstLine);
                                newSegmentsGenerated[currentRegionNumber].Remove(secondLine);
                                i = 0;
                                j = -1;
                            }
                            else if (firstLine.BasePoint == secondLine.EndPoint)
                            {
                                newSegmentsGenerated[currentRegionNumber].Add(new LineSegment(firstLine.EndPoint, secondLine.BasePoint));
                                newSegmentsGenerated[currentRegionNumber].Remove(firstLine);
                                newSegmentsGenerated[currentRegionNumber].Remove(secondLine);
                                i = 0;
                                j = -1;
                            }
                            else if (firstLine.EndPoint == secondLine.EndPoint)
                            {
                                newSegmentsGenerated[currentRegionNumber].Add(new LineSegment(firstLine.BasePoint, secondLine.BasePoint));
                                newSegmentsGenerated[currentRegionNumber].Remove(firstLine);
                                newSegmentsGenerated[currentRegionNumber].Remove(secondLine);
                                i = 0;
                                j = -1;
                            }
                            else if (firstLine.EndPoint == secondLine.BasePoint)
                            {
                                newSegmentsGenerated[currentRegionNumber].Add(new LineSegment(firstLine.BasePoint, secondLine.EndPoint));
                                newSegmentsGenerated[currentRegionNumber].Remove(firstLine);
                                newSegmentsGenerated[currentRegionNumber].Remove(secondLine);
                                i = 0;
                                j = -1;
                            }
                        }
                    }
                }

                //now we need to add the new lineSegments to our plane region
                foreach (LineSegment toAdd in newSegmentsGenerated[currentRegionNumber])
                {
                    slicedPlanes[currentRegionNumber].Add(toAdd);
                }
            }
        }

        /// <summary>
        /// Finds the intersection between this polygon and the line
        /// </summary>
        /// <param name="passedLine">The line to see if it intersects</param>
        /// <returns>Returns the point of intersection or null if it does not intersect</returns>
        public new Point Intersection(Line passedLine)
        {
            Point intersection = ((Plane)this).Intersection(passedLine);

            if(intersection != null && this.ContainsInclusive(intersection))
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
        public new LineSegment Intersection(Plane plane)
        {
            List<Point> pointsOfIntersection = new List<Point>();
            foreach(LineSegment segment in this.LineSegments)
            {
                Point point = plane.Intersection(segment);
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

                intersections.Add(edge.Intersection(passedLine));
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
            Point intersection = this.Intersection(passedLine);
            if (intersection != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns whether or not the given line and polygon intersect or are coplanar and intersect on the plane
        /// </summary>
        /// <param name="passedLine"></param>
        /// <returns></returns>
        public new bool DoesIntersect(Line passedLine)
        {
            //if the line is on the plane
            if (((Plane)this).Contains(passedLine))
            {
                //check if it intersects our boundaries
                foreach (LineSegment segment in this.LineSegments)
                {
                    if (passedLine.DoesIntersect(segment))
                    {
                        return true;
                    }
                }
            }

            return DoesIntersectNotCoplanar(passedLine);
        }

        /// <summary>
        /// Returns true if the point is contained within this PlaneRegion, Does not include the boundaries!
        /// </summary>
        /// <param name="passedPoint">The point to see if it is in this PlaneRegion</param>
        /// <returns>returns true if the Point is in this PlaneRegion and false if it is not</returns>
        public bool ContainsExclusive(Point passedPoint)
        {
            return (ContainsInclusive(passedPoint) && !Touches(passedPoint));
        }

        /// <summary>
        /// Returns true if the point is on the PlaneRegion, including on its boundaries
        /// </summary>
        /// <param name="passedPoint">The point to see if it is in this PlaneRegion</param>
        /// <returns>returns true if the Point is in this PlaneRegion or on its boundaries and false if it is not</returns>
        public bool ContainsInclusive(Point passedPoint)
        {
            //check if it is in our plane first
            if (((Plane)this).Contains(passedPoint))
            {
                //now check if it is in the bounds each line at a time
                foreach (LineSegment line in LineSegments)
                {
                    //find the plane perpendicular to this plane that represents the side we are on

                    //find the direction of the plane's normal by crossing the line's direction and the plane's normal
                    Vector divisionPlaneNormal = this.NormalVector.UnitVector(DistanceType.Inch).CrossProduct(line.UnitVector(DistanceType.Inch));

                    //now make it into a plane with the given normal and a point on the line so that it is alligned with the line
                    Plane divisionPlane = new Plane(divisionPlaneNormal.Direction, line.BasePoint);

                    //if the point is on the side outside of our region we know it is not in it and can return
                    if (!divisionPlane.PointIsOnSameSideAs(passedPoint, CenterPoint))
                    {
                        //since its inclusive, if the point is on the plane its still good
                        if (!divisionPlane.Contains(passedPoint))
                            return false;
                    }
                }

                //if it is on the right side of all the sides than we know the point must be inside the region
                return true;
            }

            //if its not in the Plane than it is obviously not in the PlaneRegion
            return false;
        }

        /// <summary>
        /// determines if this polygon contains the entirety of the other polygon
        /// </summary>
        /// <param name="polygon"></param>
        /// <returns></returns>
        public new bool Contains(Polygon polygon)
        {
            //First check all vertices
            foreach(Point vertex in polygon.Vertices)
            {
                if (!this.ContainsInclusive(vertex))
                {
                    return false;
                }
            }

            //if this is convex than we're done
            if (this.IsConvex)
            {
                return true;
            }

            //if not, we have to check that none of the outside vertices are inside the interior polygon
            foreach(Point vertex in this.Vertices)
            {
                if (polygon.ContainsExclusive(vertex))
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
        public bool DoesContainLineSegment(LineSegment lineSegment)
        {
            List<Point> points = IntersectionCoplanarPoints(lineSegment);
            int numberOfIntersections = points.Count;
            if (numberOfIntersections <= 2)
            {
                 return true;
            }
            else
            {
                for (int i = 1; 2*i < numberOfIntersections; i++)
                {
                    LineSegment segment = new LineSegment(points[2 * i - 1], points[2 * i]);
                    if (!_doesContainSegmentAlongBoundary(segment))
                    {
                        return false;
                    }
                }
                return true;
            }
           
        }

        /// <summary>
        /// Finds a vertex of this polygon that is not contained by the given plane
        /// </summary>
        /// <param name="planeNotToFindTheVertexOn">The plane to find a vertex that is not contained by</param>
        /// <returns>Returns one of the vertices that was not contained by the given plane or null if none were found</returns>
        public Point FindVertexNotOnTheGivenPlane(Plane planeNotToFindTheVertexOn)
        {
            foreach (Point vertex in this.Vertices)
            {
                if (!planeNotToFindTheVertexOn.Contains(vertex))
                {
                    return vertex;
                }
            }

            return null;
        }
        // Returns a normalVector of the polygon.
        // or the zero vector, if the polygon has no sides.
        private Vector _getUnitNormalVector()
        {
            if (this.isValidPolygon())
            {
                Point last = this.Vertices[Vertices.Count - 1];
                Point first = BasePoint;
                Point second = this.Vertices[1];
                Vector vector1 = new Vector(last, first);
                Vector vector2 = new Vector(first, second);
                Vector normal = vector1.CrossProduct(vector2);
                return new Vector(this.BasePoint, normal/normal.Magnitude.Inches);
            }
            throw new Exception("No normal Vector!");
        }

        //Determines the plane which we will rotate and project onto.
        private Plane _planeWithSmallestAngleBetween()
        {
            double angleXY = this.SmallestAngleBetween(Plane.XY).Degrees;
            double angleXZ = this.SmallestAngleBetween(Plane.XZ).Degrees;
            double angleYZ = this.SmallestAngleBetween(Plane.YZ).Degrees;

            double smallest = Math.Min(Math.Min(angleXY, angleXZ), angleYZ);
            if (angleXY == smallest)
            {
                return Plane.XY;
            }
            else if (angleYZ == smallest)
            {
                return Plane.YZ;
            }
            return Plane.XZ;
        }
      
        //Returns the necessary rotation, before we project
        private Rotation _rotationOfPlaneWithSmallestAngleBetweenOntoXYPlane()
        {
            Plane plane = _planeWithSmallestAngleBetween();
            if (plane == Plane.YZ)
            {
                return new Rotation(new Line(Direction.Up), new Angle(AngleType.Degree, 90));
            }
            else if (plane == Plane.XZ)
            {
                return new Rotation(new Line(Direction.Right), new Angle(AngleType.Degree, 90));
            }
            else
            {
                return new Rotation();
            }
        }

        private Polygon _projectOntoXYPlane()
        {
            List<Point> projectedVertices = new List<Point>();
            foreach(Point vertex in Vertices)
            {
                projectedVertices.Add(new Point(vertex.X, vertex.Y, new Distance()));
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
        private Area _findArea()
        {
            Rotation rotation = _rotationOfPlaneWithSmallestAngleBetweenOntoXYPlane();
            Polygon rotated = this.Rotate(rotation);
            Polygon projected = rotated._projectOntoXYPlane();

            List<Point> vertices = projected.Vertices;

            Area sum = new Area(AreaType.InchesSquared, 0);
            Point previousVertex = vertices[vertices.Count - 1];

            foreach(Point vertex in vertices)
            {
                sum += previousVertex.X * vertex.Y - vertex.X * previousVertex.Y;
                previousVertex = vertex;
            }

            Angle angle = rotated.SmallestAngleBetween(Plane.XY);
            
            double area = sum.InchesSquared / (2 * Math.Cos(angle.Radians));
            
            return new Area(AreaType.InchesSquared, area);
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
            double areaOfProjected = projected._findArea().InchesSquared;
            Distance xComp = sumX / (6 * areaOfProjected);
            Distance yComp = sumY / (6 * areaOfProjected);

            Point centroidOfProjected = new Point(xComp, yComp);

            Line lineOfProjection = new Line(Direction.Out, centroidOfProjected);
            Point centroidRotated = ((Plane)rotated).Intersection(lineOfProjection);
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

        #endregion
    }
}