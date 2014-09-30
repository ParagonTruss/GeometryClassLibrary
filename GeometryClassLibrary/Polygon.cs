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
        #region Fields and Properties


        public List<LineSegment> PlaneBoundaries
        {
            get { return _planeBoundaries; }
            set { _planeBoundaries = value; }
        }
        private List<LineSegment> _planeBoundaries;

        public Area Area
        {
            get
            {
                return _planeBoundaries.FindAreaOfPolygon();
            }
        }

        /// <summary>
        /// Returns the centoid (center point) of the Polygon
        /// </summary>
        /// <returns>the region's center as a point</returns>
        public Point Centroid()
        {
            //the centorid is the average of all the points
            Dimension xSum = new Dimension();
            Dimension ySum = new Dimension();
            Dimension zSum = new Dimension();

            //we count each point twice
            //the reason why we have to add all of the points twice is because we do not know which way the 
            //boundaries may be facing, so if we only add the beginPoints we may get one point twice and skip a point
            int count = PlaneBoundaries.Count * 2;

            //sum up each of the points
            foreach (LineSegment line in PlaneBoundaries)
            {
                xSum += line.BasePoint.X + line.EndPoint.X;
                ySum += line.BasePoint.Y + line.EndPoint.Y;
                zSum += line.BasePoint.Z + line.EndPoint.Z;
            }

            //now divide it by the number of points to find the average values
            return PointGenerator.MakePointWithMillimeters(xSum.Millimeters / count, ySum.Millimeters / count, zSum.Millimeters / count);

        }

        #endregion

        #region Overloaded Operators

        /// <summary>
        /// Not a perfect equality operator, is only accurate up to the Dimension Class's accuracy
        /// </summary>
        public static bool operator ==(Polygon region1, Polygon region2)
        {
            if ((object)region1 == null || (object)region2 == null)
            {
                if ((object)region1 == null && (object)region2 == null)
                {
                    return true;
                }
                return false;
            }
            return region1.Equals(region2);
        }

        /// <summary>
        /// Not a perfect equality operator, is only accurate up to the Dimension Class's accuracy
        /// </summary>
        public static bool operator !=(Polygon region1, Polygon region2)
        {
            if (region1 == null || region2 == null)
            {
                if (region1 == null && region2 == null)
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
            Polygon comparableRegion = null;

            //try to cast the object to a Plane Region, if it fails then we know the user passed in the wrong type of object
            try
            {
                comparableRegion = (Polygon)obj;
                bool areEqual = true;
                foreach (LineSegment segment in comparableRegion.PlaneBoundaries)
                {
                    if (!_planeBoundaries.Contains(segment))
                    {
                        areEqual = false;
                    }

                }

                return areEqual;
            }
            catch
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

        #region Constructors
        /// <summary>
        /// Zero constructor
        /// </summary>
        public Polygon()
        {

        }

        /// <summary>
        /// Defines a plane region using the given boundaries as long as the line segments form a closed region
        /// </summary>
        /// <param name="passedBoundaries"></param>
        public Polygon(List<LineSegment> passedBoundaries)
            : this(passedBoundaries, 3) { }

        /// <summary>
        /// Defines a plane region using the given boundaries as long as the line segments form a closed region within the given tolerance
        /// </summary>
        /// <param name="passedBoundaries"></param>
        public Polygon(List<LineSegment> passedBoundaries, int passedNumberOfDecimalsToCheck)
            : base(passedBoundaries)
        {
            List<LineSegment> roundedBoundaryList = (List<LineSegment>)passedBoundaries.RoundAllPoints(passedNumberOfDecimalsToCheck);
            bool isClosed = roundedBoundaryList.DoFormClosedRegion();
            bool areCoplanar = roundedBoundaryList.AreAllCoplanar();

            if (isClosed && areCoplanar)
            {
                _planeBoundaries = roundedBoundaryList;
            }
        }

        /// <summary>
        /// creates a new Polygon that is a copy of the inputted Polygon
        /// </summary>
        /// <param name="passedBoundaries"></param>
        public Polygon(Polygon planeToCopy)
            //note: we do not need to call List<LineSegment>(newplaneToCopy.PlaneBoundaries) because it does this in the base case for 
            //constructing a plane fron a List<LineSegment>
            : this(planeToCopy.PlaneBoundaries, 3) { }


        public Polygon(List<Point> passedPoints)
            : this(passedPoints.MakeIntoLineSegmentsThatMeet()) { }
        #endregion

        #region Methods

        public Polygon Shift(Shift passedShift)
        {
            return new Polygon(this.PlaneBoundaries.Shift(passedShift));
        }

        /// <summary>
        /// Rotates the plane region about the given axis by the specified angle. Point values are rounded to 6 decimal places to make sure the boundaries still meet after rotating.
        /// </summary>
        /// <param name="passedAxisLine"></param>
        /// <param name="passedRotationAngle"></param>
        /// <returns></returns>
        public Polygon Rotate(Line passedAxisLine, Angle passedRotationAngle)
        {
            List<LineSegment> newBoundaryList = new List<LineSegment>();
            foreach (LineSegment segment in _planeBoundaries)
            {
                newBoundaryList.Add(segment.Rotate(passedAxisLine, passedRotationAngle));
            }
            newBoundaryList = (List<LineSegment>)newBoundaryList.RoundAllPoints(5);
            return new Polygon(newBoundaryList);
        }

        public Polygon Translate(Vector passedDirectionVector, Dimension passedDisplacement)
        {
            List<LineSegment> newBoundaryList = new List<LineSegment>();
            foreach (LineSegment segment in _planeBoundaries)
            {
                newBoundaryList.Add(segment.Translate(passedDirectionVector, passedDisplacement));
            }
            return new Polygon(newBoundaryList);
        }

        public Polygon SmallestRectangleThatCanSurroundThisShape()
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
        /// <param name="dimension"></param>
        /// <returns></returns>
        public Polyhedron Extrude(Dimension dimension)
        {
            //find two lines that are not parallel
            LineSegment firstLine = _planeBoundaries[0];
            LineSegment secondLine = null;
            foreach (var lineseg in _planeBoundaries)
            {
                if (!lineseg.IsParallelTo(firstLine))
                {
                    secondLine = lineseg;
                }
            }

            if (secondLine == null)
            {
                throw new Exception("There are no LineSegments in this plane region that are not parallel");
            }

            Vector normalVector = firstLine.DirectionVector.CrossProduct(secondLine.DirectionVector);

            //create back Polygon
            List<LineSegment> backPolygonLines = new List<LineSegment>();
            List<LineSegment> otherPolygonLines = new List<LineSegment>();

            foreach (var linesegment in _planeBoundaries)
            {
                Point newBackBasePoint = linesegment.BasePoint.Translate(normalVector.XComponentOfDirection, normalVector.YComponentOfDirection, normalVector.ZComponentOfDirection);
                Point newBackEndPoint = linesegment.EndPoint.Translate(normalVector.XComponentOfDirection, normalVector.YComponentOfDirection, normalVector.ZComponentOfDirection);
                backPolygonLines.Add(new LineSegment(newBackBasePoint, newBackEndPoint));

                LineSegment newNormalLine = new LineSegment(newBackBasePoint, linesegment.BasePoint);

                if (!otherPolygonLines.Contains(newNormalLine))
                {
                    otherPolygonLines.Add(newNormalLine);
                }

                newNormalLine = new LineSegment(newBackEndPoint, linesegment.BasePoint);

                if (!otherPolygonLines.Contains(newNormalLine))
                {
                    otherPolygonLines.Add(newNormalLine);
                }
            }

            Polyhedron returnGeometry = new Polyhedron();
            returnGeometry.Polygons.Add(this);
            returnGeometry.Polygons.Add(new Polygon(backPolygonLines));

            //take all coplanar lines and create a Polygon from 


            throw new NotImplementedException();


        }


        #endregion
        





        /// <summary>
        /// Returns true if the Polygon is valid (is a closed region and the LineSegments are all coplaner)
        /// </summary>
        /// <returns>returns true if the LineSegments form a closed area and they are all coplaner</returns>
        public bool isValidPolygon(int passedNumberOfDecimalsToCheck)
        {
            List<LineSegment> roundedBoundaryList = (List<LineSegment>)PlaneBoundaries.RoundAllPoints(passedNumberOfDecimalsToCheck);
            bool isClosed = roundedBoundaryList.DoFormClosedRegion();
            bool areCoplanar = roundedBoundaryList.AreAllCoplanar();

            if (isClosed && areCoplanar)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Finds and returns a Point that is on this PlaneRegion, but not on its boundaries, and the PlaneRegion passed in
        /// </summary>
        /// <param name="otherPlane">Other plane region to find a shared point with</param>
        /// <returns>returns a point which both planes share on this plane but not on its boundaries or null if they do not overlap</returns>
        public Point SharedPointNotOnThisPolygonsBoundary(Polygon otherPlane)
        {
            //check the centroids
            if (otherPlane.ContainsInclusive(this.Centroid()))
            {
                return this.Centroid();
            }
            if (this.ContainsExclusive(otherPlane.Centroid()))
            {
                return otherPlane.Centroid();
            }

            //if we still havent found it try looking at the veticies of the otherPlane
            foreach (Point vertex in otherPlane.PlaneBoundaries.GetAllPoints())
            {
                if (this.ContainsExclusive(vertex))
                {
                    return vertex;
                }
            }

            //still still havent found it check if the sides overlap
            //we know one line at least must intersect this planes boundaries twice
            foreach (LineSegment line in otherPlane.PlaneBoundaries)
            {
                //keep track of our intersection
                Point firstIntesect = null;

                //look at all the bounding planes
                foreach (LineSegment otherLine in this.PlaneBoundaries)
                {
                    //see if they intersect
                    Point intersection = line.Intersection(otherLine);
                    if (intersection != null)
                    {
                        //if we havent found a first intersection
                        if (firstIntesect == null)
                        {
                            firstIntesect = intersection;
                        }
                        //if we already found one than this is the second and we can interpolate the point between
                        else
                        {
                            //find the point between them by making a line between them and then finding the midpoint of it
                            LineSegment betweenIntersects = new LineSegment(firstIntesect, intersection);

                            //we have to return here instead of breaking because breaking will only take us out of onw
                            return betweenIntersects.MidPoint;
                        }
                    }
                }
            }

            //if we didnt find any return null
            return null;
        }

        /// <summary>
        /// This finds and returns the Polygon where the two Polygons overlap or null if they do not 
        /// overlap (or if they are only touching - the overlap region has an area o 0). The plane this function 
        /// is called on must a convex polygon or else the function will not return the proper region
        /// </summary>
        /// <param name="planeToBeClipped">The Polygon that will be clipped (can be either a convex or concave polygon)</param>
        /// <returns>Returns the Polygon that represents where the two Polygons overlap or null if they do not overlap
        /// or only touch</returns>
        public Polygon OverlappingPolygon(Polygon planeToBeClipped)
        {
            //if they are coplanar
            if (((Plane)this).Contains(planeToBeClipped))
            {
                //using the the idea of the Sutherland-Hodgman algoritm

                //create the plane we will be trimming so we dont mess up the original one
                Polygon overlapping = new Polygon(planeToBeClipped);

                //find a point where they overlap
                Point referencePoint = this.SharedPointNotOnThisPolygonsBoundary(planeToBeClipped);

                //if we couldnt find a shared point than they must not overlap
                if (referencePoint != null)
                {
                    //then find where the sides overlap
                    foreach (Line divisionLine in this.PlaneBoundaries)
                    {
                        List<Polygon> slicedPlane = overlapping.Slice(divisionLine);

                        //findout which one we want to keep and we dont need the other part
                        overlapping = slicedPlane[0];
                        if (slicedPlane[1].ContainsInclusive(referencePoint))
                        {
                            overlapping = slicedPlane[1];
                        }
                    }
                    //make sure that our Polygon is valid (coplanar and enclosed) and that the area is not equal 
                    //to zero (this would mean the Polygons are only touching not overlapping so we dont want to return it) 
                    //before returning the region ( for now we just say if there are more than two sides - area not implemented correctly
                    //right now and this should work for how the function is set up)
                    if (overlapping.isValidPolygon(3) && overlapping.PlaneBoundaries.Count > 2)
                        return overlapping;
                }
            }

            //if we fail to find a valid Polygon of intersection return null
            return null;
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
            Vector divisionPlaneNormal = this.NormalVector.CrossProduct(slicingLine.DirectionVector);
            Plane divisionPlane = new Plane(slicingLine.BasePoint, divisionPlaneNormal);

            return this.Slice(slicingLine, divisionPlane);
        }

        /// <summary>
        /// This function Slices a plane and returns both halves of the plane, with the larger piece returning first. If the
        /// Plane does not intersect this Polygon it returns a copy of the original plane region in a list
        /// </summary>
        /// <param name="slicingPlane">the plane to use to slice this Polygon where they intersect</param>
        /// <returns>returns a List of the two plane Regions that represent the slices region with the region with the larger area first</returns>
        public List<Polygon> Slice(Plane slicingPlane)
        {
            throw new NotImplementedException();

            Line slicingLine = new Line();
            //Line slicingLine = slicingPlane.IntersectionLine(this);

            return this.Slice(slicingLine, slicingPlane);
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
        private List<Polygon> Slice(Line slicingLine, Plane slicingPlane)
        {
            //make sure the line is in this plane or else it shouldnt slice
            if (((Plane)this).Contains(slicingLine))
            {
                //          NOTE:
                //index 0 is our insidePlaneRegion
                //index 1 is our outsidePlaneRegion

                //create our two regions that we will modify and return
                List<Polygon> slicedPlanes = new List<Polygon>() { new Polygon(this), new Polygon(this) };

                //now get our reference point - one that we know is on a side and not on the plane (using the normal is an easy consistent way)
                Point referencePoint = slicingPlane.BasePoint + slicingPlane.NormalVector.DirectionPoint;

                //keep track of all the new lines we added so that we can connect them later on (one for each region returned)
                List<List<LineSegment>> newSegmentsGenerated = new List<List<LineSegment>>() { new List<LineSegment>(), new List<LineSegment>() };

                //we have to keep track of lines to move and do it after the foreach loop because we cant change the list
                //while we are looping through it (immutable) (one for each region returned)
                List<List<LineSegment>> toRemove = new List<List<LineSegment>>() { new List<LineSegment>(), new List<LineSegment>() };

                //loop through each segment in the planeRegion to see if and where it needs to be sliced
                foreach (LineSegment line in slicedPlanes[0].PlaneBoundaries)
                {
                    //find where or if the linesegment overlaps the clipping line
                    Point intersectPoint = line.Intersection(slicingLine);

                    //if there is an interception than we need to clip the line
                    if (intersectPoint != null)
                    {
                        //if the line does not have one of its points on the plane than we need to slice it 
                        if (!slicingPlane.Contains(line.EndPoint) && !slicingPlane.Contains(line.BasePoint))
                        {
                            //we know we will always get two because we already checked and confirmed intersect
                            List<LineSegment> lineSliced = line.Slice(intersectPoint);

                            //guess as what line part is the outside;
                            LineSegment outsidePart = lineSliced[0];
                            LineSegment insidePart = lineSliced[1];

                            //check to see if our "guess" was right

                            //if the base point of the original line is on the other side as the reference point then that line that 
                            //contains it is the outside part of the line
                            if (!slicingPlane.PointIsOnSameSideAs(line.BasePoint, referencePoint))
                            {
                                //if the basepoint is on the lineSegment than that is the inside part
                                if (line.BasePoint.IsOnLineSegment(lineSliced[1]))
                                {
                                    outsidePart = lineSliced[1];
                                    insidePart = lineSliced[0];
                                }
                            }
                            //if the end point of the original line is on the other side of the reference point then that line 
                            //that contains it is actually the outside line
                            else if (!slicingPlane.PointIsOnSameSideAs(line.EndPoint, referencePoint))
                            {
                                //this time if the endpoint is on the lineSegment than that is the inside part
                                if (line.EndPoint.IsOnLineSegment(lineSliced[1]))
                                {
                                    outsidePart = lineSliced[1];
                                    insidePart = lineSliced[0];
                                }
                            }
                            //otherwise our guess was right

                            //now we need to deal with the projection on the lines and both halves since we need to keep both 
                            //sections the region is split into 

                            //Deal with the outsidePlane and outside part of the line
                            //first project it(inside line) onto the division line(this is for the outsidePlane) 
                            LineSegment projectedLineForOutside = insidePart.ProjectOntoLine(slicingLine);

                            //and then add it to our new segments list if its not zero length
                            if (projectedLineForOutside.Length != new Dimension())
                            {
                                newSegmentsGenerated[1].Add(projectedLineForOutside);
                            }

                            //get the same line in the outside planeregion as we already have in line for the inside region
                            //and then change it to the outside part of the line
                            int indexOfLine = slicedPlanes[1].PlaneBoundaries.IndexOf(line);
                            slicedPlanes[1].PlaneBoundaries[indexOfLine] = outsidePart;


                            //Deal with the insidePlane and the inside part of the line
                            //now do it all again for the outside line too (this is for the insidePlane)
                            LineSegment projectedLineForInside = outsidePart.ProjectOntoLine(slicingLine);
                            if (projectedLineForInside.Length != new Dimension())
                            {
                                newSegmentsGenerated[0].Add(projectedLineForInside);
                            }

                            //now we can change the intersecting line to the inside line
                            //we have to do it part by part because line is immutable during a foreach loop so we cannot reassign it, only modify it
                            //we also have to change basepoint and endpoint other wise it will just translate the lineSegment
                            line.BasePoint = insidePart.BasePoint;
                            line.Length = insidePart.Length;
                        }
                        //if there is a point on the plane than we need to remove for one region if its on the other side
                        else
                        {
                            //if the endpoint is on the slicing plane than we can determine which side based on the basepoint
                            if (slicingPlane.Contains(line.EndPoint))
                            {
                                //if the base point is on the inside than we should cut it out of the outside
                                if (slicingPlane.PointIsOnSameSideAs(line.BasePoint, referencePoint))
                                {
                                    toRemove[1].Add(line);
                                }
                                //if it wasnt on the inside than it was on the outside and we need to remove it from the inside
                                else
                                {
                                    toRemove[0].Add(line);
                                }
                            }
                            //if the endpoint wasnt on the line than the basepoint was and we can determine the side with the endpoint
                            else
                            {
                                //if the end point is on the inside than we should cut it out of the outside
                                if (slicingPlane.PointIsOnSameSideAs(line.EndPoint, referencePoint))
                                {
                                    toRemove[1].Add(line);
                                }
                                //if it wasnt on the inside than it was on the outside and we need to remove it from the inside
                                else
                                {
                                    toRemove[0].Add(line);
                                }
                            }
                        }
                    }
                    //if it doesnt intersect at all we are either completely on the inside side or the out side
                    else
                    {
                        //if one of the points is on the outside than both of them must be at this point
                        //so we need to project it for the inisde region and leave it for the outside one
                        if (!slicingPlane.PointIsOnSameSideAs(line.BasePoint, referencePoint))
                        {
                            //add the projection to our new segments list for the inside region
                            LineSegment projectedLine = line.ProjectOntoLine(slicingLine);
                            if (projectedLine.Length != new Dimension())
                            {
                                newSegmentsGenerated[0].Add(projectedLine);
                            }

                            //and now we add it to the toRemove list of the inside plane so we know to get rid of it so 
                            //it doesnt cause us problems later on
                            toRemove[0].Add(line);
                        }
                        //we know that it is either on the other side or on the plane so if we check that it is
                        //not on the plane than we know it must be on the same side as the reference point
                        //so we need to leave it for the inside region and project it for the outside one
                        else if (!slicingPlane.Contains(line.BasePoint))
                        {
                            //add the projection to our new segments list for the outside region
                            LineSegment projectedLine = line.ProjectOntoLine(slicingLine);
                            if (projectedLine.Length != new Dimension())
                            {
                                newSegmentsGenerated[1].Add(projectedLine);
                            }

                            //and now we add it to the toRemove list for the outside plane so we know to get rid of it so it 
                            //doesnt caus us problems later on
                            toRemove[1].Add(line);
                        }
                    }
                }

                //now consolidate the new lines combining them to as few as posible (should only be one line at the end because all 
                //the projections were on the same line) and then add them to the polygon

                //combine any of the lines that share the same point so we dont have reduntent/more segments than necessary
                //we can do this because we know that they are all along the same line so if they share a point they are
                //just an extension of the other one (they all should be exensions of the same line)
                for (int currentRegionNumber = 0; currentRegionNumber < newSegmentsGenerated.Count; currentRegionNumber++)
                {
                    //remove any segments we need to from our overlapping polygon first
                    foreach (LineSegment lineToRemove in toRemove[currentRegionNumber])
                    {
                        slicedPlanes[currentRegionNumber].PlaneBoundaries.Remove(lineToRemove);
                    }

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
                        slicedPlanes[currentRegionNumber].PlaneBoundaries.Add(toAdd);
                    }
                }

                //now return them in opposite order
                slicedPlanes.Sort();
                slicedPlanes.Reverse();
                return slicedPlanes;
            }
            //if we were not in the plane than we return a copy of the orignal plane (this)
            return new List<Polygon>() { this };
        }

        private bool Contains(Line slicingLine)
        {
            throw new NotImplementedException();
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
                foreach (LineSegment line in PlaneBoundaries)
                {
                    //find the plane perpendicular to this plane that represents the side we are on

                    //find the direction of the plane's normal by crossing the line's direction and the plane's normal
                    Vector divisionPlaneNormal = NormalVector.CrossProduct(line.DirectionVector);

                    //now make it into a plane with the given normal and a point on the line so that it is alligned with the line
                    Plane divisionPlane = new Plane(line.BasePoint, divisionPlaneNormal);

                    //if the point is on the side outside of our region we know it is not in it and can return
                    if (!divisionPlane.PointIsOnSameSideAs(passedPoint, Centroid()))
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
        /// Checks if the point is touching the PlaneRegion (aka if it is on the boundaries of the planeRegion)
        /// </summary>
        /// <param name="passedPoint">Point to check if it is touching</param>
        /// <returns>Returns true if the point touches the PlaneRegion and false if it is not on the boundaries</returns>
        public bool Touches(Point passedPoint)
        {
            //check each of our boundaries if the point is on the LineSegment
            foreach (LineSegment line in this.PlaneBoundaries)
            {
                if (passedPoint.IsOnLineSegment(line))
                {
                    return true;
                }
            }
            return false;
        }
    }
}