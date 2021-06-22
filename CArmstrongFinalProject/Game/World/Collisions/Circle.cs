/* Circle.cs
 * Description: Circle.cs is a class file that the Circle class.
 * The Circle class is a class that contains fields and functions 
 * relating to a Circle.
 * 
 * Revision History
 *      Colin Armstrong, 2019.12.04: Created
 */
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CArmstrongFinalProject
{
    /// <summary>
    /// Circle: This is a class that contains fields and functions relating to a Circle.
    /// </summary>
    class Circle
    {
        private float radius;
        public float Radius { get => radius; }
        private float x;
        public float X { get => x; }
        private float y;
        public float Y { get => y; }

        /// <summary>
        /// The primary constructor of the Circle class.
        /// </summary>
        /// <param name="positionX">The X positon of the center of the Circle.</param>
        /// <param name="positionY">The Y positon of the center of the Circle.</param>
        /// <param name="radius">The length of the radius of the Circle.</param>
        public Circle(float positionX, float positionY, float radius)
        {
            this.x = positionX;
            this.y = positionY;
            this.radius = radius;
        }

        /// <summary>
        /// Intersects is a method that checks if a rectangle intersects with the Circle object.
        /// </summary>
        /// <param name="rectToCheck">The rectangle to check for intersection with.</param>
        /// <returns>Returns true if Rectangle and Circle do intersect, else returns false if no intersection.</returns>
        public bool Intersects(Rectangle rectToCheck)
        {
            Point[] corners = new Point[]
            {
                new Point(rectToCheck.Top, rectToCheck.Left),
                new Point(rectToCheck.Top, rectToCheck.Right),
                new Point(rectToCheck.Bottom, rectToCheck.Right),
                new Point(rectToCheck.Bottom, rectToCheck.Left)
            };

            foreach (Point corner in corners)
            {
                if (ContainsPoint(corner))
                    return true;
            }

            if (x - Radius > rectToCheck.Right || x + Radius < rectToCheck.Left)
                return false;

            if (y - Radius > rectToCheck.Bottom || y + Radius < rectToCheck.Top)
                return false;

            return true;
        }

        /// <summary>
        /// Intersects is a method that checks if two Circle objects intersect with each other.
        /// This is done by comparing the distance between the two centers of the circles to the 
        /// combined radius of the two circles.
        /// </summary>
        /// <param name="incomingCircle"></param>
        /// <returns>Returns true if the Circles do intersect, else returns false if no intersection.</returns>
        public bool Intersects(Circle incomingCircle)
        {
            Vector2 circle1Center = new Vector2(incomingCircle.X, incomingCircle.Y);
            Vector2 circle2Center = new Vector2(X, Y);
            return Vector2.Distance(circle1Center, circle2Center) < Radius + incomingCircle.Radius;
        }

        /// <summary>
        /// ContainsPoint checks whether the current Circle object contains a specified point.
        /// It checks it by measuring if the distance from the center to the point is greater than the radius or not.
        /// </summary>
        /// <param name="point">The point location to check.</param>
        /// <returns>Returns a boolean of whether or not a circle contains a point.</returns>
        public bool ContainsPoint(Point point)
        {
            Vector2 distVect = new Vector2(point.X - X, point.Y - Y);
            return distVect.Length() <= Radius;
        }

        /// <summary>
        /// PositionOnEdgeOfCircle is a method that returns a Vector2 of coordinates of the position on the edge of 
        /// the circle object based on parameters.
        /// </summary>
        /// <param name="angle">The angle of the location of the edge of the circle.</param>
        /// <param name="percentfromEdge">The percentage distance from the edge of the circle.</param>
        /// <returns></returns>
        public Vector2 PositionOnEdgeOfCircle(float angle, float percentfromEdge)
        {
            double r = Radius * percentfromEdge;
            double x = X + r * Math.Sin((angle));
            double y = Y + r * Math.Cos((angle));
            return new Vector2((float)x, (float)y);
        }
    }
}
