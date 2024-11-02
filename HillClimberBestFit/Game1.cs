using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace HillClimberBestFit
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;
        private MouseState previousMs;
        private List<double[]> pointX;
        private List<double> pointY;
        HillClimberLine line;
        private Random random;
        PerceptronBestFitLine perceptronLine;
        Func<double, double, double> errorFunc;
        int averageX;
        int averageY;
        Point initialPoint;
        float slope;
        float yIntercept;
        double currentError;
        double[] yPoints;
        double[][] drawxs;
        double[] drawys;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }


        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = 800;
            _graphics.PreferredBackBufferWidth = 1000;
            _graphics.ApplyChanges();

            currentError = double.MaxValue;
            pointX = new List<double[]>();
            pointY = new List<double>();
            random = new Random();
            //line = new HillClimberLine(points, random);
            errorFunc = ErrorFunc;
            perceptronLine = new PerceptronBestFitLine(1, 0.01, random, errorFunc);


            base.Initialize();
        }

        private double ErrorFunc(double actual, double desired)
        {
            return Math.Pow(desired - actual, 2);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseState ms = Mouse.GetState();
            if (ms.LeftButton == ButtonState.Pressed && previousMs.LeftButton == ButtonState.Released && IsActive)
            {
                pointX.Add([ms.X]);
                pointY.Add(ms.Y);
                currentError = double.MaxValue;
            }

            if (pointX.Count > 1)
            {
                double[][] xPoints = new double[pointX.Count][];
                for(int i = 0; i < pointX.Count; i++)
                {
                    xPoints[i] = new double[1];
                    xPoints[i][0] = pointX[i][0];
                }

                currentError = perceptronLine.TrainWithHillClimbing(xPoints, pointY.ToArray(), currentError);
            }
            drawxs = [[0],[GraphicsDevice.Viewport.Width]];
            drawys = perceptronLine.Compute(drawxs);

            //if (pointX.Count() > 1)
            //    {
            //        averageY = 0;
            //        averageX = 0;
            //        initialPoint = pointX[0][0];
            //        foreach (var point in points)
            //        {
            //            averageX += point[0].X;
            //            averageY += point[0].Y;
            //        }
            //        averageX /= points.Count();
            //        averageY /= points.Count();
            //        slope = (float)(averageY - initialPoint.Y) / (averageX - initialPoint.X);
            //        //y = mx + b -> b = y - mx
            //        yIntercept = averageY - (slope * averageX);



            // yleft=Percptron(xleft)

            //currentError = perceptronLine.TrainWithHillClimbing(currentError);
            // }

            previousMs = ms;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            //manual best fit
            //initial point, averaged point, calcultae line


            spriteBatch.Begin();
            spriteBatch.DrawLine((float)drawxs[0][0], (float)drawys[0], (float)drawxs[1][0], (float)drawys[1], Color.Black);
            //if(points.Count() > 1)
            //{
            //    spriteBatch.DrawLine(new(0, yIntercept), 4000f, slope, Color.Black);
            //}

            Window.Title = $"{currentError}";


            //foreach (var point in points)
            //{
            //    spriteBatch.DrawEllipse(point.ToVector2(), new(5, 5), 20, Color.Black, 10f);
            //}


            for (int i = 0; i < pointX.Count; i++)
            {
                spriteBatch.DrawEllipse(new Vector2((float)pointX[i][0], (float)pointY[i]), new(5, 5), 20, Color.Black, 10f);


            }



            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
