using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitClassLibrary;


namespace GeometryClassLibrary
{
    public class PlaneRegion : Plane, IComparable<PlaneRegion>
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

        #endregion

        #region Overloaded Operators

        /// <summary>
        /// Not a perfect equality operator, is only accurate up to the Dimension Class's accuracy
        /// </summary>
        public static bool operator ==(PlaneRegion region1, PlaneRegion region2)
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
        public static bool operator !=(PlaneRegion region1, PlaneRegion region2)
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
            PlaneRegion comparableRegion = null;

            //try to cast the object to a Plane Region, if it fails then we know the user passed in the wrong type of object
            try
            {
                comparableRegion = (PlaneRegion)obj;
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

        #endregion

        #region Constructors

        /// <summary>
        /// Defines a plane region using the given boundaries as long as the line segments form a closed region
        /// </summary>
        /// <param name="passedBoundaries"></param>
        public PlaneRegion(List<LineSegment> passedBoundaries)
            : this(passedBoundaries, 3) { }

        /// <summary>
        /// Defines a plane region using the given boundaries as long as the line segments form a closed region within the given tolerance
        /// </summary>
        /// <param name="passedBoundaries"></param>
        public PlaneRegion(List<LineSegment> passedBoundaries, int passedNumberOfDecimalsToCheck)
            : base(passedBoundaries)
        {
            List<LineSegment> roundedBoundaryList = (List<LineSegment>) passedBoundaries.RoundAllPoints(passedNumberOfDecimalsToCheck);
            bool isClosed = roundedBoundaryList.DoFormClosedRegion();
            bool areCoplanar = roundedBoundaryList.AreAllCoplanar();

            if (isClosed && areCoplanar)
            {
                _planeBoundaries = roundedBoundaryList;
            }
        }

        /// <summary>
        /// creates a new PlaneRegion that is a copy of the inputted PlaneRegion
        /// </summary>
        /// <param name="passedBoundaries"></param>
        public PlaneRegion(PlaneRegion planeToCopy)
            //note: we do not need to call List<LineSegment>(newplaneToCopy.PlaneBoundaries) because it does this in the base case for 
            //constructing a plane fron a List<LineSegment>
            : this(planeToCopy.PlaneBoundaries, 3) { }


        public PlaneRegion(List<Point> passedPoints)
            : this(passedPoints.MakeIntoLineSegmentsThatMeet()) { }
        #endregion

        #region Methods

        /// <summary>
        /// Rotates the plane region about the given axis by the specified angle. Point values are rounded to 6 decimal places to make sure the boundaries still meet after rotating.
        /// </summary>
        /// <param name="passedAxisLine"></param>
        /// <param name="passedRotationAngle"></param>
        /// <returns></returns>
        public PlaneRegion Rotate(Line passedAxisLine, Angle passedRotationAngle)
        {
            List<LineSegment> newBoundaryList = new List<LineSegment>();
            foreach (LineSegment segment in _planeBoundaries)
            {
                newBoundaryList.Add(segment.Rotate(passedAxisLine, passedRotationAngle));
            }
            newBoundaryList = (List<LineSegment>) newBoundaryList.RoundAllPoints(5);
            return new PlaneRegion(newBoundaryList);
        }

        public PlaneRegion Translate(Vector passedDirectionVector, Dimension passedDisplacement)
        {
            List<LineSegment> newBoundaryList = new List<LineSegment>();
            foreach (LineSegment segment in _planeBoundaries)
            {
                newBoundaryList.Add(segment.Translate(passedDirectionVector, passedDisplacement));
            }
            return new PlaneRegion(newBoundaryList);
        }

        public PlaneRegion SmallestRectangleThatCanSurroundThisShape()
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
        public Solid Extrude(Dimension dimension)
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

            //create back planeRegion
            List<LineSegment> backPlaneRegionLines = new List<LineSegment>();
            List<LineSegment> otherPlaneRegionLines = new List<LineSegment>();

            foreach (var linesegment in _planeBoundaries)
            {
                Point newBackBasePoint = linesegment.BasePoint.Translate(normalVector.XComponentOfDirection, normalVector.YComponentOfDirection, normalVector.ZComponentOfDirection);
                Point newBackEndPoint = linesegment.EndPoint.Translate(normalVector.XComponentOfDirection, normalVector.YComponentOfDirection, normalVector.ZComponentOfDirection);
                backPlaneRegionLines.Add(new LineSegment(newBackBasePoint, newBackEndPoint));

                LineSegment newNormalLine = new LineSegment(newBackBasePoint, linesegment.BasePoint);

                if (!otherPlaneRegionLines.Contains(newNormalLine))
                {
                    otherPlaneRegionLines.Add(newNormalLine);
                }

                newNormalLine = new LineSegment(newBackEndPoint, linesegment.BasePoint);

                if (!otherPlaneRegionLines.Contains(newNormalLine))
                {
                    otherPlaneRegionLines.Add(newNormalLine);
                }
            }

            Solid returnGeometry = new Solid();
            returnGeometry.PlaneRegions.Add(this);
            returnGeometry.PlaneRegions.Add(new PlaneRegion(backPlaneRegionLines));

            //take all coplanar lines and create a PlaneRegion from 


            throw new NotImplementedException();


        }

        public int CompareTo(PlaneRegion other)
        {
            throw new NotImplementedException();
        }

        

        #endregion

        public LineSegment LineSegment
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public PlaneRegion Shift(Shift passedShift)
        {
           return new PlaneRegion( this.PlaneBoundaries.Shift(passedShift));
        }

        /// <summary>
        /// Returns the centoid (center point) of the PlaneRegion
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

        /// <summary>
        /// Returns true if the PlaneRegion is valid (is a closed region and the LineSegments are all coplaner)
        /// </summary>
        /// <returns>returns true if the LineSegments form a closed area and they are all coplaner</returns>
        public bool isValidPlaneRegion(int passedNumberOfDecimalsToCheck)
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
        /// Returns true if the point is contained within this PlaneRegion
        /// </summary>
        /// <param name="passedPoint">The point to see if it is in this PlaneRegion</param>
        /// <returns>returns true if the Point is in this PlaneRegion and false if it is not</returns>
        public new bool Contains(Point passedPoint)
        {
            //check if it is in our plane first
            if(base.Contains(passedPoint))
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
        /// This finds and returns the PlaneRegion where the two PlaneRegions overlap or null if they do not 
        /// overlap (or if they are only touching - the overlap region has an area o 0). The plane this function 
        /// is called on must a convex polygon or else the function will not return the proper region
        /// </summary>
        /// <param name="planeToBeClipped">The PlaneRegion that will be clipped (can be either a convex or concave polygon)</param>
        /// <returns>Returns the PlaneRegion that represents where the two PlaneRegions overlap or null if they do not overlap
        /// or only touch</returns>
        public PlaneRegion OverlappingPlaneRegion(PlaneRegion planeToBeClipped)
        {
            //if they are coplanar
            if (this.Contains(planeToBeClipped))
            {
                //using the the idea of the Sutherland-Hodgman algoritm

                //create the plane we will be trimming so we dont mess up the original one
                PlaneRegion overlapping = new PlaneRegion(planeToBeClipped);

                foreach (Line divisionLine in this.PlaneBoundaries)
                {
                    Vector divisionPlaneNormal = this.NormalVector.CrossProduct(divisionLine.DirectionVector);
                    Plane divisionPlane = new Plane(divisionLine.BasePoint, divisionPlaneNormal);

                    //keep track of all the new lines we added so that we can connect them if we need to
                    List<LineSegment> newSegmentsGenerated = new List<LineSegment>();

                    //we have to keep track of lines to move and do it after the foreach loop because we cant change the list
                    //while we are looping through it (immutable)
                    List<LineSegment> toRemove = new List<LineSegment>();

                    foreach (LineSegment line in overlapping.PlaneBoundaries)
                    {
                        //find where or if the linesegment overlaps the clipping line
                        Point intersectPoint = line.Intersection(divisionLine);
                        //if there is an interception than we need to clip the line
                        if (intersectPoint != null)
                        {
                            List<LineSegment> lineSliced = line.Slice(intersectPoint);
 
                            // if the base point is on the "wrong" side of the plane that it is the one we need to remove
                            if (!divisionPlane.PointIsOnSameSideAs(line.BasePoint, this.Centroid()))
                            {
                                //check to make sure its not on the plane first because then we want to leave it unchanged
                                if (!divisionPlane.Contains(line.BasePoint))
                                {
                                    //but first we need to project the part of the line outside the boundary onto the plane so we can make a line segment
                                    //that connects the new points so we have an enclosed region

                                    //find out which line contains the basePoint (and is thus the outside line)
                                    LineSegment outsidePart = lineSliced[0];
                                    LineSegment insidePart = lineSliced[1];
                                    //if the second one doesnt contain the basePoint than the first one must so we can leave it 
                                    if (line.BasePoint.IsOnLineSegment(lineSliced[1]))
                                    {
                                        outsidePart = lineSliced[1];
                                        insidePart = lineSliced[0];
                                    }

                                    //now project it onto the division line 
                                    LineSegment projectedLine = outsidePart.ProjectOntoLine(divisionLine);

                                    //and then add it to our new segments list if its not zero length
                                    if (projectedLine.Length != new Dimension())
                                    {
                                        newSegmentsGenerated.Add(projectedLine);
                                    }

                                    //now we can change the intersecting line to the inside line
                                    //have to do it part by part because line is immutable during a foreach loop
                                    //we also have to change basepoint and endpoint other wise it will just translate the lineSegment
                                    line.BasePoint = insidePart.BasePoint;
                                    line.EndPoint = insidePart.EndPoint;
                                }
                            }
                            //if the base point wasnt on the wrong side than we need to check if the endPoint is on the wrong side
                            else if (!divisionPlane.PointIsOnSameSideAs(line.EndPoint, this.Centroid()))
                            {
                                //make sure the point isnt on the plane
                                if (!divisionPlane.Contains(line.EndPoint))
                                {
                                    //We also need to project this first

                                    //find out which line contains the endPoint (and is thus the outside line)
                                    //we dont need both segments because of the nature of the lineSegment class because if we
                                    //move the endpoint the length and stuff changes, but if we move the base point the whole line moves
                                    LineSegment outsidePart = lineSliced[0];
                                    //if the second one doesnt contain the endPoint than the first one must so we can leave it 
                                    if (line.EndPoint.IsOnLineSegment(lineSliced[1]))
                                    {
                                        outsidePart = lineSliced[1];
                                    }

                                    //now project it onto the division line 
                                    LineSegment projectedLine = outsidePart.ProjectOntoLine(divisionLine);

                                    //and then add it to our new segments list if its not zero length
                                    if (projectedLine.Length != new Dimension())
                                    {
                                        newSegmentsGenerated.Add(projectedLine);
                                    }

                                    //now change the basepoint of our intersecting line
                                    //we dont have to change the base point because it is the same as before
                                    line.EndPoint = intersectPoint;
                                }
                            }
                        }
                        //if it doesnt intersect at all we are either completely on the right side or the wrong side
                        else
                        {
                            //if one of the points is on the wrong side than both of them must be at this point
                            if (!divisionPlane.PointIsOnSameSideAs(line.BasePoint, this.Centroid()))
                            {
                                //add the projection to our new segments list for them
                                LineSegment projectedLine = line.ProjectOntoLine(divisionLine);
                                if (projectedLine.Length != new Dimension())
                                {
                                    newSegmentsGenerated.Add(projectedLine);
                                }

                                //and now we add it to the toRemove list so we know to get rid of it so it doesnt caus us problems later on
                                toRemove.Add(line);
                            }
                        }
                    }

                    //remove any segments we need to from our overlapping polygon
                    foreach (LineSegment lineToRemove in toRemove)
                    {
                        overlapping.PlaneBoundaries.Remove(lineToRemove);
                    }

                    //now consolidate the new lines and then add them to the polygon

                    //combine any of the lines that share the same point so we dont have reduntent/more segments than necessary
                    //we can do this because we know that they are all along the same line so if they share a point they are
                    //just an extension of the other one
                    for (int i = 0; i < newSegmentsGenerated.Count; i++)
                    {
                        for (int j = 0; j < newSegmentsGenerated.Count; j++)
                        {
                            //get our lines for this round of checks
                            LineSegment firstLine = newSegmentsGenerated[i];
                            LineSegment secondLine = newSegmentsGenerated[j];

                            //if its not the same line (or equivalent - if it is we just ignore it for now and it will work iself out as other ones combine)
                            if (firstLine != secondLine)
                            {
                                //if two points match then combine them and add the new one to the list
                                //then remove the two old ones
                                //then we need to restart it at i = 0, j = -1 (-1 because it will increment at the end and then be back to 0) otherwise 
                                //we may skip some or gout out of bounds
                                if (firstLine.BasePoint == secondLine.BasePoint)
                                {
                                    newSegmentsGenerated.Add(new LineSegment(firstLine.EndPoint, secondLine.EndPoint));
                                    newSegmentsGenerated.Remove(firstLine);
                                    newSegmentsGenerated.Remove(secondLine);
                                    i = 0;
                                    j = -1;
                                }
                                else if (firstLine.BasePoint == secondLine.EndPoint)
                                {
                                    newSegmentsGenerated.Add(new LineSegment(firstLine.EndPoint, secondLine.BasePoint));
                                    newSegmentsGenerated.Remove(firstLine);
                                    newSegmentsGenerated.Remove(secondLine);
                                    i = 0;
                                    j = -1;
                                }
                                else if (firstLine.EndPoint == secondLine.EndPoint)
                                {
                                    newSegmentsGenerated.Add(new LineSegment(firstLine.BasePoint, secondLine.BasePoint));
                                    newSegmentsGenerated.Remove(firstLine);
                                    newSegmentsGenerated.Remove(secondLine);
                                    i = 0;
                                    j = -1;
                                }
                                else if (firstLine.EndPoint == secondLine.BasePoint)
                                {
                                    newSegmentsGenerated.Add(new LineSegment(firstLine.BasePoint, secondLine.EndPoint));
                                    newSegmentsGenerated.Remove(firstLine);
                                    newSegmentsGenerated.Remove(secondLine);
                                    i = 0;
                                    j = -1;
                                }
                            }
                        }
                    }

                    //now we need to add the new lineSegments to our plane region
                    foreach (LineSegment toAdd in newSegmentsGenerated)
                    {
                        overlapping.PlaneBoundaries.Add(toAdd);
                    }

                }
                //make sure that our planeRegion is valid (coplanar and enclosed) and that the area is not equal 
                //to zero (this would mean the planeRegions are only touching not overlapping so we dont want to return it) 
                //before returning the region ( for now we just say if there are more than two sides - area not implemented correctly
                //right now and this should work for how the function is set up)
                if (overlapping.isValidPlaneRegion(3) && overlapping.PlaneBoundaries.Count > 2)
                    return overlapping;
            }

            //if we fail to find a valid planeRegion of intersection return null
            return null;
        }

        public List<PlaneRegion> Slice(Line slicingLine)
        {


        }

        public List<PlaneRegion> Slice(Plane slicingPlane)
        {
            throw new NotImplementedException();
            //once plane.IntersectionLine(plane) is created, this is simple to implement like so
            Line intersectionLine = new Line();
            //Line intersectionLine = slicingPlane.IntersectionLine(this);
            return this.Slice(intersectionLine);
        }


    }
}
