using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace _2_convex_hull
{
    class ConvexHullSolver
    {
        System.Drawing.Graphics g;
        System.Windows.Forms.PictureBox pictureBoxView;
        public ConvexHullSolver(System.Drawing.Graphics g, System.Windows.Forms.PictureBox pictureBoxView)
        {
            this.g = g;
            this.pictureBoxView = pictureBoxView;
        }
        public void Refresh()
        {
            // Use this especially for debugging and whenever you want to see what you have drawn so far
            pictureBoxView.Refresh();
        }
        public void Pause(int milliseconds)
        {
            // Use this especially for debugging and to animate your algorithm slowly
            pictureBoxView.Refresh();
            System.Threading.Thread.Sleep(milliseconds);
        }

        private void TestMakeTriangleList()
        {
            //----------------------------------------------------------------------
            //3 points all on same x-value: (or, 1st and Last points on same x-value:)
            //----------------------------------------------------------------------
            System.Drawing.PointF p1 = new System.Drawing.PointF(1, 4);
            System.Drawing.PointF p2 = new System.Drawing.PointF(1, 3);
            System.Drawing.PointF p3 = new System.Drawing.PointF(1, 2);

            List<System.Drawing.PointF> listOfPoints = new List<System.Drawing.PointF>();
            listOfPoints.Add(p1);
            listOfPoints.Add(p2);
            listOfPoints.Add(p3);

            List<System.Drawing.PointF> resultList = makeTriangleList(ref listOfPoints);

            //MY TEST CODE: MAKE SURE THESE ARE IN ORDER OF ASCENDING X VALUES!!!
            Console.WriteLine("Make sure the following are in ascending order:");
            foreach (System.Drawing.PointF p in resultList)
            {
                Console.WriteLine(p.Y);
            }
            Console.WriteLine("End");




            //----------------------------------------------------------------------
            //First 2 points all on same x-value:
            //----------------------------------------------------------------------


            System.Drawing.PointF p4 = new System.Drawing.PointF(1, 1);
            System.Drawing.PointF p5 = new System.Drawing.PointF(1, 3);
            System.Drawing.PointF p6 = new System.Drawing.PointF(2, 2);

            List<System.Drawing.PointF> listOfPoints2 = new List<System.Drawing.PointF>();
            listOfPoints2.Add(p4);
            listOfPoints2.Add(p5);
            listOfPoints2.Add(p6);

            List<System.Drawing.PointF> resultList2 = makeTriangleList(ref listOfPoints2);

            //MY TEST CODE: MAKE SURE THESE ARE IN ORDER OF ASCENDING X VALUES!!!
            Console.WriteLine("Make sure it's in this cycle: 13 22 11");
            foreach (System.Drawing.PointF p in resultList2)
            {
                Console.Write(p.X);
                Console.WriteLine(p.Y);
            }
            Console.WriteLine("End");





            //----------------------------------------------------------------------
            //Last two points all on same x-value:
            //----------------------------------------------------------------------

            System.Drawing.PointF p7 = new System.Drawing.PointF(1, 1);
            System.Drawing.PointF p8 = new System.Drawing.PointF(1, 3);
            System.Drawing.PointF p9 = new System.Drawing.PointF(2, 2);

            List<System.Drawing.PointF> listOfPoints3 = new List<System.Drawing.PointF>();
            listOfPoints3.Add(p7);
            listOfPoints3.Add(p8);
            listOfPoints3.Add(p9);

            List<System.Drawing.PointF> resultList3 = makeTriangleList(ref listOfPoints3);

            //MY TEST CODE: MAKE SURE THESE ARE IN ORDER OF ASCENDING X VALUES!!!
            Console.WriteLine("Make sure it's in this cycle: 22, 11, 13");
            foreach (System.Drawing.PointF p in resultList3)
            {
                Console.Write(p.X);
                Console.WriteLine(p.Y);
            }
            Console.WriteLine("End");




            //----------------------------------------------------------------------
            //All on different x-values:
            //----------------------------------------------------------------------

            System.Drawing.PointF p10 = new System.Drawing.PointF(1, 5);
            System.Drawing.PointF p11 = new System.Drawing.PointF(2, 6);
            System.Drawing.PointF p12 = new System.Drawing.PointF(3, 4);

            List<System.Drawing.PointF> listOfPoints4 = new List<System.Drawing.PointF>();
            listOfPoints4.Add(p10);
            listOfPoints4.Add(p11);
            listOfPoints4.Add(p12);

            List<System.Drawing.PointF> resultList4 = makeTriangleList(ref listOfPoints4);

            //MY TEST CODE: MAKE SURE THESE ARE IN ORDER OF ASCENDING X VALUES!!!
            Console.WriteLine("Make sure it's in this cycle: 15 26 34");
            foreach (System.Drawing.PointF p in resultList4)
            {
                Console.Write(p.X);
                Console.WriteLine(p.Y);
            }
            Console.WriteLine("End");
        }
        private void TestFindLeftMostPoint()
        {
            throw new NotImplementedException();
        }
        private void TestFindRightMostPoint()
        {
            throw new NotImplementedException();
        }
        private void TestGetSlope()
        {
            throw new NotImplementedException();
        }
        private void TestGetNextInCycle()
        {
            throw new NotImplementedException();
        }
        private void TestConnectTheDots()
        {
            //I tested it myself, informally, and it worked.
            throw new NotImplementedException();
        }
        private void Test()
        {
            TestMakeTriangleList();
            //TestFindLeftMostPoint();
            //TestFindRightMostPoint();
            //TestGetNextInCycle();
            //TestGetSlope();
        }


        public void Solve(List<System.Drawing.PointF> pointList)
        {
            if (pointList.Count < 2)
            {
                return;
            }

           //Sort the pointList in order of ascending X-values.
           pointList.Sort(delegate(System.Drawing.PointF p1, System.Drawing.PointF p2) { return p1.X.CompareTo(p2.X); });
           List<System.Drawing.PointF> hullPoints = generateHull(ref pointList);
           connectTheDots(hullPoints);
           Refresh();
        }

       
        private void connectTheDots(List<System.Drawing.PointF> hullPoints)
        {
            Pen pen = new Pen(Color.Aqua);
            for (int i = 1; i < hullPoints.Count; i++)
            {
                g.DrawLine(pen, hullPoints[i - 1], hullPoints[i]);
            }
            //Draw final line from last point to first point.
            g.DrawLine(pen, hullPoints[hullPoints.Count - 1], hullPoints[0]);
            Refresh();
        }



        /**
        * PRECONDITIONS: 
        *       1. The list of points is sorted in order of ascending x-values.
        *       2. The list of points has either 2 or 3 points.
        * POSTCONDITIONS:
        *       1. The returned list of points is made with copies of the previous points (not references).
        *       2. The list contains all of the points on the original list.
        *       3. The points are in the sequence that makes a hull if you were to draw an edge between each of the points
        *          (and assume the first and last points make an edge also).  The ordering is clockwise mathematically 
        *          (meaning, goes from origin, then up, then right, then down, then left) but visually, it will look
        *          mirrored. Meaning, it goes from origin, then down, then right, then up, then left. So visually,
        *          it will look counterclockwise.
        *       4. THERE IS NO GUARANTEE ABOUT WHERE THE CYCLE BEGINS OR ENDS.
        *        
        */
        private List<System.Drawing.PointF> generateHull(ref List<System.Drawing.PointF> pointList)
        {
            //========================= ASSERTIONS ============================================================
            //Assert that PointList.Count is never 0 or 1.
            if (pointList.Count < 2)
            {
                throw new Exception();
            }


            //========================== BASE CASES ============================================================
            //Put the points into "clockwise" order.
            if (pointList.Count == 2)
            {
                return pointList;
            }
            if (pointList.Count == 3)
            {
                return makeTriangleList(ref pointList);
            }


            //=========================== RECURSIVE SECTION ===================================================

            //Divide the list into two sublists.
            List<System.Drawing.PointF> leftList = new List<System.Drawing.PointF>();
            List<System.Drawing.PointF> rightList = new List<System.Drawing.PointF>();
            createSublists(ref pointList, ref leftList, ref rightList);
            return combineHulls(generateHull(ref leftList), generateHull(ref rightList));
        }


        //==================================================================================================================
        //======================================= COMBINE HULLS ====================================================
        //==================================================================================================================


      /**
       * PRECONDITIONS:
       *        1. The left and right lists are both in clockwise ordering. 
       */
        private List<System.Drawing.PointF> combineHulls(List<System.Drawing.PointF> leftList, List<System.Drawing.PointF> rightList)
        {
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // Find the UpperTangent.
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            //1) Find rightmost point on leftList.
            int leftPointIndex= findRightMostPointOnLeftHull(ref leftList);
 
            //2) Find leftmost point on rightList.
            int rightPointIndex = findLeftMostPointOnRightHull(ref rightList);
            
            //3) Keep advancing to the next point on rightList (using getNextInCycle) until the slope stops increasing.
            //While the slope of the next point on the right INCREASES the slope... then store THAT right point's index.
            float originalSlope = getSlope(leftList[leftPointIndex], rightList[rightPointIndex]);
            float nextSlope = getSlope(leftList[leftPointIndex], rightList[getNextInCycle(rightPointIndex, ref rightList)]);
            while (originalSlope < nextSlope)
            {
                rightPointIndex = getNextInCycle(rightPointIndex, ref rightList);
                originalSlope = getSlope(leftList[leftPointIndex], rightList[rightPointIndex]);
                nextSlope = getSlope(leftList[leftPointIndex], rightList[getNextInCycle(rightPointIndex, ref rightList)]);
            }

            //4) Keep advancing to the previous point on leftlist (using getPreviousInCycle) until the slope stops decreasing.
            //While the slope of the next point on the left DECREASES the slope... then store THAT left point's index.
            originalSlope = getSlope(leftList[leftPointIndex], rightList[rightPointIndex]);
            nextSlope = getSlope(leftList[getPreviousInCycle(leftPointIndex, ref leftList)], rightList[rightPointIndex]);
            while (originalSlope > nextSlope)
            {
                leftPointIndex = getPreviousInCycle(leftPointIndex, ref leftList);
                originalSlope = getSlope(leftList[leftPointIndex], rightList[rightPointIndex]);
                nextSlope = getSlope(leftList[getPreviousInCycle(leftPointIndex, ref leftList)], rightList[rightPointIndex]);
            }

            //ASSUMPTION AT THIS POINT: rightPointIndex and leftPointIndex have the points of the UPPER TANGENT.
            //I will store these in a special place.
            int upperLeft = leftPointIndex;
            int upperRight = rightPointIndex;



            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // Find the LowerTangent.
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            //1) Find rightmost point on leftList.
            leftPointIndex = findRightMostPointOnLeftHull(ref leftList);

            //2) Find leftmost point on rightList.
            rightPointIndex = findLeftMostPointOnRightHull(ref rightList);

            //3) Keep advancing to the previous point on rightList (using getPreviousInCycle) until the slope stops decreasing.

            //While the slope of the previous (counter-clockwise) point on the right DECREASES the slope... then store THAT right point's index.
            originalSlope = getSlope(leftList[leftPointIndex], rightList[rightPointIndex]);
            nextSlope = getSlope(leftList[leftPointIndex], rightList[getPreviousInCycle(rightPointIndex, ref rightList)]);
            while (originalSlope > nextSlope)
            {
                rightPointIndex = getPreviousInCycle(rightPointIndex, ref rightList);
                nextSlope = getSlope(leftList[leftPointIndex], rightList[getPreviousInCycle(rightPointIndex, ref rightList)]);
                originalSlope = getSlope(leftList[leftPointIndex], rightList[rightPointIndex]);
            }

            //While the slope of the next (clockwise) point on the left INCREASES the slope... then store THAT left point's index.
            originalSlope = getSlope(leftList[leftPointIndex], rightList[rightPointIndex]);
            nextSlope = getSlope(leftList[getNextInCycle(leftPointIndex, ref leftList)], rightList[rightPointIndex]);
            while (originalSlope < nextSlope)
            {
                leftPointIndex = getNextInCycle(leftPointIndex, ref leftList);
                originalSlope = getSlope(leftList[leftPointIndex], rightList[rightPointIndex]);
                nextSlope = getSlope(leftList[getNextInCycle(leftPointIndex, ref leftList)], rightList[rightPointIndex]);
            }

            //ASSUMPTION AT THIS POINT: rightPointIndex and leftPointIndex have the points of the LOWER TANGENT.
            //I will store these in a special place.
            int lowerLeft = leftPointIndex;
            int lowerRight = rightPointIndex;

            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // Use the Upper and Lower Tangent's points to create the new, clockwise-ordered list, omitting the middle points.
            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            List<System.Drawing.PointF> combinedHullList = new List<System.Drawing.PointF>();
            
            //Add lower left... all the way up to (and not including) upperLeft.
            for (int i = lowerLeft; i != upperLeft; i = getNextInCycle(i, ref leftList))
            {
                combinedHullList.Add(leftList[i]);
            }

            //Add upperLeft.
            combinedHullList.Add(leftList[upperLeft]);

            //Add upperRight... all the way up to (and not including) lowerRight.
            for (int i = upperRight; i != lowerRight; i = getNextInCycle(i, ref rightList))
            {
                combinedHullList.Add(rightList[i]);
            }

            //Add lowerRight.
            combinedHullList.Add(rightList[lowerRight]);

            return combinedHullList;
        }


        //==================================================================================================================
        //====================== FINDERS (of LeftMostPointOnRightHull, RightMostPointOnLeftHull) ===========================
        //==================================================================================================================


        private int findLeftMostPointOnRightHull(ref List<System.Drawing.PointF> rightList)
        {
            //Just uses 0 as a starting point because it goes through the whole list and will
            //replace it when it finds a farther-left point index.
            int leftMostIndex = 0;
            for(int i = 0; i < rightList.Count; i++)
            {
                if (rightList[i].X < rightList[leftMostIndex].X)
                {
                    leftMostIndex = i;
                }
            }
            return leftMostIndex;
        }

        private int findRightMostPointOnLeftHull(ref List<System.Drawing.PointF> leftList)
        {
            //Just uses leftList[0] as a starting point because it goes through the whole list and will
            //replace it when it finds a farther-right point.
            int rightMostIndex = 0;
            for(int i = 0; i < leftList.Count; i++)
            {
                if (leftList[i].X > leftList[rightMostIndex].X)
                {
                    rightMostIndex = i;
                }
            }
            return rightMostIndex;
        }






        //==================================================================================================================
        //=========================== GETTERS (of NextInCycle, PreviousInCycle, Slope) =====================================
        //==================================================================================================================


        /**
         *  PRECONDITIONS: 
         *          1. List's size is greater than 0.
         *          
         *  RETURNS:
         *          1. (an int) one more than "previous". If the next index would've been out of bounds, it'll be 0.
         *
         */
        private int getNextInCycle(int previous, ref List<System.Drawing.PointF> list)
        {
            //========================= ASSERTIONS ============================================================
            //Assert that list.Count is never 0.
            if (list.Count == 0)
            {
                throw new Exception();
            }

            //========================= NORMAL CODE ============================================================
            //Change the list's size to represent the INDEX of the last position of the list. So to do that, minus 1.
            int lastPosition = list.Count - 1;
            if (previous == lastPosition)
            {
                return 0;
            }
            else
            {
                previous++;
                return previous;
            }
        }

        /**
         *  PRECONDITIONS: 
         *          1. List's size is greater than 0.
         *          
         *  RETURNS:
         *          1. (an int) one less than "previous". If the next index would've been out of bounds, it'll be list.Count - 1.
         *
         */
        private int getPreviousInCycle(int previous, ref List<System.Drawing.PointF> list)
        {
            //========================= ASSERTIONS ============================================================
            //Assert that list.Count is never 0.
            if (list.Count == 0)
            {
                throw new Exception();
            }

            //========================= NORMAL CODE ============================================================
            //Change the list's size to represent the INDEX of the last position of the list. So to do that, minus 1.
            int lastPosition = list.Count - 1;
            if (previous == 0)
            {
                return lastPosition;
            }
            else
            {
                previous--;
                return previous;
            }
        }

        /**
         * INPUTS:
         *      1. left: any point
         *      2. right: any other point.
         *      
         * RETURNS:
         *      1. float the slope of the line connecting the two points.
         */
        private float getSlope(System.Drawing.PointF left, System.Drawing.PointF right)
        {
            //Rise / Run.   The Y and X values are stored as floats, so it will be an accurate calculation.
           return (right.Y - left.Y) / (right.X - left.X);
        }






        //==================================================================================================================
        //======================================= CREATE SUBLISTS STUFF ====================================================
        //==================================================================================================================

       /**
        * PRECONDITIONS:
        *     1. leftList and rightList are empty.  
        *     2. Original list has 2 or more elements.
        *     
        * POSTCONDITIONS: 
        *      1. Half of the elements of originalList are on leftList, and the other half are on rightList. 
        *      2. If it's an odd number of elements, the left list gets the extra element.
        */
        private void createSublists(ref List<System.Drawing.PointF> originalList,
        ref List<System.Drawing.PointF> leftList, ref List<System.Drawing.PointF> rightList)
        {
            int leftListSize = calculateLeftHullSize(originalList.Count);
            for (int i = 0; i < leftListSize; i++)
            {
                leftList.Add(originalList[i]);
            }
            for (int i = leftListSize; i < originalList.Count; i++)
            {
                rightList.Add(originalList[i]);
            }
        }

        /**
         * PARAMETERS:
         *      1. count: the size of the original list that you're splitting into two lists.
         * RETURNS:
         *      1. (an int) the size of what the left list should be.
         */
        private int calculateLeftHullSize(int count)
        {
            //If it's even... return the exact half.
            if (count % 2 == 0)
            {
                return count / 2;
            }
            //If it's odd, then there'll be one extra. The left side gets it.
            //The division should have already thrown out the remainder. (I tested it. It does.)
            else
            {
                return (count / 2) + 1;
            }
        }








        //==================================================================================================================
        //======================================= TRIANGLE LIST STUFF ======================================================
        //==================================================================================================================

        /**
          * Makes a list of nodes in clockwise order if the original list had 3 points in it.
          * 
          * POSTCONDITIONS:
          *         1. The points are NOT NECESSARILY in order of x-values.
          */
        private List<System.Drawing.PointF> makeTriangleList(ref List<System.Drawing.PointF> pointList)
        {
            //If all three points are on the same line.........
            //.....or if the first and last points are on the same line....... (because then you automatically know that the middle one is on the same line.)
            if (pointList[0].X == pointList[2].X)
            {
                return pointList;
            }
            //.....or if the first two points are on the same line.......
            else if (pointList[0].X == pointList[1].X)
            {
                return CaseFirstTwoPointsAreOnSameLine(ref pointList);
            }
            //.....or if the last two points are on the same line.......
            else if (pointList[1].X == pointList[2].X)
            {
                return CaseLastTwoPointsAreOnSameLine(ref pointList);
            }
            //CASE WHERE THEY'RE ALL ON DIFFERENT LINES:
            else
            {
                return CaseAllOnDifferentXValues(ref pointList);
            }
        }

       /**
        * Makes a list of nodes in clockwise order, for the case where the first two points are on the same line.
        *  
        * POSTCONDITIONS:
        *       1. Just so happens to return them in order of ascending x-values.
        */
        private List<System.Drawing.PointF> CaseFirstTwoPointsAreOnSameLine(ref List<System.Drawing.PointF> pointList)
        {
            //Rearrange those two points to be in order of ascending Y values if they aren't already.
            if (pointList[1].Y < pointList[0].Y)
            {
                List<System.Drawing.PointF> newList = new List<System.Drawing.PointF>();
                //Ordering matters here!!! Since 1 is lower than 0, make 1 come first.
                newList.Add(pointList[1]);
                newList.Add(pointList[0]);
                newList.Add(pointList[2]);
                return newList;
            }
            //If you got here, it's because they're already in the right order.
            return pointList;
        }

       /**
        * Makes a list of nodes in clockwise order, for the case where the last two points are on the same line.
        * 
        * POSTCONDITIONS:
        *       1. Just so happens to return them in order of ascending x-values.
        */
        private List<System.Drawing.PointF> CaseLastTwoPointsAreOnSameLine(ref List<System.Drawing.PointF> pointList)
        {
            //Rearrange those two points to be in order of ascending Y values if they aren't already.
            if (pointList[2].Y > pointList[1].Y)
            {
                List<System.Drawing.PointF> newList = new List<System.Drawing.PointF>();
                //Ordering matters here!!! Since 1 is lower than 0, make 1 come first.
                newList.Add(pointList[0]);
                newList.Add(pointList[2]);
                newList.Add(pointList[1]);
                return newList;
            }
            //If you got here, it's because they're already in the right order.
            return pointList;
        }

        /**
         * Makes a list of nodes in clockwise order, for the case where the points are all on different lines.
         * 
         * POSTCONDITIONS:
         *      1. One case makes them in order of ascending x-values, but the other doesn't.
         */
        private List<System.Drawing.PointF> CaseAllOnDifferentXValues(ref List<System.Drawing.PointF> pointList)
        {
            //Case where they're already clockwise.
            float slopeToIndexOne = getSlope(pointList[0], pointList[1]);
            float slopeToIndexTwo = getSlope(pointList[0], pointList[2]);

            if (slopeToIndexOne > slopeToIndexTwo)
            {
                return pointList;
            }
            else
            {
                List<System.Drawing.PointF> newList = new List<System.Drawing.PointF>();
                //Ordering matters here!!! Since 1 is lower than 2, make 2 come first.
                newList.Add(pointList[0]);
                newList.Add(pointList[2]);
                newList.Add(pointList[1]);
                return newList;
            }
        }



    }


}
