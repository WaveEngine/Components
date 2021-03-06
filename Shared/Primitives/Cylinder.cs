﻿// Copyright © 2018 Wave Engine S.L. All rights reserved. Use is subject to license terms.

#region Using Statements
using System;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Graphics3D;
#endregion

namespace WaveEngine.Components.Primitives
{
    /// <summary>
    /// A 3D cylinder.
    /// </summary>
    internal sealed class Cylinder : Geometric
    {
        #region Initialize

        /// <summary>
        /// Initializes a new instance of the <see cref="Cylinder" /> class.
        /// </summary>
        /// <param name="height">The height.</param>
        /// <param name="diameter">The diameter.</param>
        /// <param name="tessellation">The tessellation.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">If tessellation is less than 3.</exception>
        public Cylinder(float height, float diameter, int tessellation)
        {
            if (tessellation < 3)
            {
                throw new ArgumentOutOfRangeException("tessellation");
            }

            height /= 2;

            float radius = diameter / 2;

            // Create a ring of triangles around the outside of the cylinder.
            for (int i = 0; i <= tessellation; i++)
            {
                float percent = i / (float)tessellation;
                float angle = percent * MathHelper.TwoPi;

                float dx = (float)Math.Cos(angle);
                float dz = (float)Math.Sin(angle);

                Vector3 normal = new Vector3(dx, 0, dz);

                this.AddVertex((normal * radius) + (Vector3.Up * height), normal, new Vector2(1 - percent, 0));
                this.AddVertex((normal * radius) + (Vector3.Down * height), normal, new Vector2(1 - percent, 1));

                if (i < tessellation)
                {
                    this.AddIndex(i * 2);
                    this.AddIndex((i * 2) + 1);
                    this.AddIndex(((i * 2) + 2) % ((tessellation + 1) * 2));

                    this.AddIndex((i * 2) + 1);
                    this.AddIndex(((i * 2) + 3) % ((tessellation + 1) * 2));
                    this.AddIndex(((i * 2) + 2) % ((tessellation + 1) * 2));
                }
            }

            // Create flat triangle fan caps to seal the top and bottom.
            this.CreateCap(tessellation, height, radius, Vector3.Up);
            this.CreateCap(tessellation, height, radius, Vector3.Down);
        }

        /// <summary>
        /// Helper method that creates a triangle fan to close the ends of the cylinder.
        /// </summary>
        /// <param name="tessellation">The tessellation.</param>
        /// <param name="height">The height.</param>
        /// <param name="radius">The radius.</param>
        /// <param name="normal">The normal.</param>
        private void CreateCap(int tessellation, float height, float radius, Vector3 normal)
        {
            // Create cap indices.
            for (int i = 0; i < tessellation - 2; i++)
            {
                if (normal.Y > 0)
                {
                    this.AddIndex(this.VerticesCount);
                    this.AddIndex(this.VerticesCount + ((i + 1) % tessellation));
                    this.AddIndex(this.VerticesCount + ((i + 2) % tessellation));
                }
                else
                {
                    this.AddIndex(this.VerticesCount);
                    this.AddIndex(this.VerticesCount + ((i + 2) % tessellation));
                    this.AddIndex(this.VerticesCount + ((i + 1) % tessellation));
                }
            }

            // Create cap vertices.
            for (int i = 0; i < tessellation; i++)
            {
                float angle = i * MathHelper.TwoPi / tessellation;

                float dx = (float)Math.Cos(angle);
                float dz = (float)Math.Sin(angle);

                Vector3 iniPosition = new Vector3(dx, 0, dz);
                Vector3 position = (iniPosition * radius) + (normal * height);

                var uHorizontalCoord = (dx * 0.5f) + 0.5f;

                if (normal.Y < 0)
                {
                    uHorizontalCoord = 1 - uHorizontalCoord;
                }

                this.AddVertex(position, normal, new Vector2(uHorizontalCoord, (dz * 0.5f) + 0.5f));
            }
        }
        #endregion
    }
}
