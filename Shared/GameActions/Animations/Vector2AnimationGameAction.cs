﻿// Copyright © 2018 Wave Engine S.L. All rights reserved. Use is subject to license terms.

#region Using Statements
using System;
using System.Diagnostics.CodeAnalysis;
using WaveEngine.Common.Math;
using WaveEngine.Framework;
#endregion

namespace WaveEngine.Components.GameActions
{
    /// <summary>
    /// Game action of a single float value
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "*", Justification = "because yes")]
    public class Vector2AnimationGameAction : UpdatableGameAction
    {
        #region Fields

        /// <summary>
        /// The instances
        /// </summary>
        private static int instances;

        /// <summary>
        /// The update action of the
        /// </summary>
        protected Action<Vector2> updateAction;

        /// <summary>
        /// The total time of the game action
        /// </summary>
        private TimeSpan totalTime;

        /// <summary>
        /// Initial value
        /// </summary>
        protected Vector2 from;

        /// <summary>
        /// End value
        /// </summary>
        protected Vector2 to;

        /// <summary>
        /// The entity
        /// </summary>
        private Entity entity;

        /// <summary>
        /// The easing function
        /// </summary>
        private EaseFunction easeFunction;

        /// <summary>
        /// The updater behavior
        /// </summary>
        private GameActionUpdaterBehavior updaterBehavior;

        /// <summary>
        /// The ellapsed time
        /// </summary>
        private TimeSpan ellapsedTime;
        #endregion

        #region Properties
        #endregion

        #region Initialize
        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2AnimationGameAction"/> class.
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <param name="from">Initial value</param>
        /// <param name="to">End value</param>
        /// <param name="time">The time of the animation</param>
        /// <param name="ease">Easing function</param>
        protected Vector2AnimationGameAction(Entity entity, Vector2 from, Vector2 to, TimeSpan time, EaseFunction ease)
            : this(entity, from, to, time, ease, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2AnimationGameAction"/> class.
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <param name="from">Initial value</param>
        /// <param name="to">End value</param>
        /// <param name="time">The time of the animation</param>
        /// <param name="ease">Easing function</param>
        /// <param name="updateAction">The action when needs to be updated</param>
        public Vector2AnimationGameAction(Entity entity, Vector2 from, Vector2 to, TimeSpan time, EaseFunction ease, Action<Vector2> updateAction)
            : base("Vector2Animation" + instances++, entity.Scene)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            this.entity = entity;
            this.totalTime = time;
            this.from = from;
            this.to = to;
            this.easeFunction = ease;
            this.updateAction = updateAction;
        }

        /// <summary>
        /// Updates the single game action
        /// </summary>
        /// <param name="gameTime">The ellapsed gameTime</param>
        public override void Update(TimeSpan gameTime)
        {
            this.ellapsedTime += gameTime;

            if (this.ellapsedTime > this.totalTime)
            {
                this.AssignValueTo(this.to);
                this.updaterBehavior.StopAction(this);
                this.PerformCompleted();
            }
            else
            {
                var value = this.DeltaFunction(this.easeFunction, this.from, this.to - this.from, (float)this.ellapsedTime.TotalSeconds, (float)this.totalTime.TotalSeconds);
                this.AssignValueTo(value);
            }
        }

        /// <summary>
        /// Assign value to method
        /// </summary>
        /// <param name="value">current value</param>
        private void AssignValueTo(Vector2 value)
        {
            if (this.updateAction != null)
            {
                this.updateAction(value);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Cancels the game action
        /// </summary>
        protected override void PerformCancel()
        {
            base.PerformCancel();

            this.updaterBehavior.StopAction(this);
        }

        /// <summary>
        /// Notifies the skip.
        /// </summary>
        /// <returns>If the action is skipped susscessfully</returns>
        protected override bool PerformSkip()
        {
            this.AssignValueTo(this.to);
            this.updaterBehavior.StopAction(this);
            return base.PerformSkip();
        }

        /// <summary>
        /// Ons the run.
        /// </summary>
        protected override void PerformRun()
        {
            this.ellapsedTime = TimeSpan.Zero;

            if (this.updaterBehavior == null)
            {
                var updater = this.entity.FindComponent<GameActionUpdaterBehavior>();
                if (updater == null)
                {
                    updater = new GameActionUpdaterBehavior();
                    this.entity.AddComponent(updater);
                }

                this.updaterBehavior = updater;
            }

            this.updaterBehavior.BeginAction(this);
        }

        /// <summary>
        /// Delta Function method
        /// </summary>
        /// <param name="function">ease function</param>
        /// <param name="from">Start value</param>
        /// <param name="to">End value</param>
        /// <param name="time">Current time</param>
        /// <param name="totalTime">Animation duration</param>
        /// <returns>Delta value</returns>
        public Vector2 DeltaFunction(EaseFunction function, Vector2 from, Vector2 to, float time, float totalTime)
        {
            Vector2 delta = Vector2.Zero;

            switch (this.easeFunction)
            {
                case EaseFunction.BackInEase:
                    delta = this.BackInEase(time, from, to, totalTime);
                    break;
                case EaseFunction.BackInOutEase:
                    delta = this.BackInOutEase(time, from, to, totalTime);
                    break;
                case EaseFunction.BackOutEase:
                    delta = this.BackOutEase(time, from, to, totalTime);
                    break;
                case EaseFunction.BounceInEase:
                    delta = this.BounceInEase(time, from, to, totalTime);
                    break;
                case EaseFunction.BounceOutEase:
                    delta = this.BounceOutEase(time, from, to, totalTime);
                    break;
                case EaseFunction.BounceInOutEase:
                    delta = this.BounceInOutEase(time, from, to, totalTime);
                    break;
                case EaseFunction.CircleInEase:
                    delta = this.CircleInEase(time, from, to, totalTime);
                    break;
                case EaseFunction.CircleOutEase:
                    delta = this.CircleOutEase(time, from, to, totalTime);
                    break;
                case EaseFunction.CircleInOutEase:
                    delta = this.CircleInOutEase(time, from, to, totalTime);
                    break;
                case EaseFunction.CubicInEase:
                    delta = this.CubicInEase(time, from, to, totalTime);
                    break;
                case EaseFunction.CubicOutEase:
                    delta = this.CubicOutEase(time, from, to, totalTime);
                    break;
                case EaseFunction.CubicInOutEase:
                    delta = this.CubicInOutEase(time, from, to, totalTime);
                    break;
                case EaseFunction.ElasticInEase:
                    delta = this.ElasticInEase(time, from, to, totalTime);
                    break;
                case EaseFunction.ElasticOutEase:
                    delta = this.ElasticOutEase(time, from, to, totalTime);
                    break;
                case EaseFunction.ElasticInOutEase:
                    delta = this.ElasticInOutEase(time, from, to, totalTime);
                    break;
                case EaseFunction.ExponentialInEase:
                    delta = this.ExponentialInEase(time, from, to, totalTime);
                    break;
                case EaseFunction.ExponentialOutEase:
                    delta = this.ExponentialOutEase(time, from, to, totalTime);
                    break;
                case EaseFunction.ExponentialInOutEase:
                    delta = this.ExponentialInOutEase(time, from, to, totalTime);
                    break;
                case EaseFunction.QuadraticInEase:
                    delta = this.QuadraticInEase(time, from, to, totalTime);
                    break;
                case EaseFunction.QuadraticOutEase:
                    delta = this.QuadraticOutEase(time, from, to, totalTime);
                    break;
                case EaseFunction.QuadraticInOutEase:
                    delta = this.QuadraticInOutEase(time, from, to, totalTime);
                    break;
                case EaseFunction.QuarticInEase:
                    delta = this.QuarticInEase(time, from, to, totalTime);
                    break;
                case EaseFunction.QuarticOutEase:
                    delta = this.QuarticOutEase(time, from, to, totalTime);
                    break;
                case EaseFunction.QuarticInOutEase:
                    delta = this.QuarticInOutEase(time, from, to, totalTime);
                    break;
                case EaseFunction.QuinticInEase:
                    delta = this.QuinticInEase(time, from, to, totalTime);
                    break;
                case EaseFunction.QuinticOutEase:
                    delta = this.QuinticOutEase(time, from, to, totalTime);
                    break;
                case EaseFunction.QuinticInOutEase:
                    delta = this.QuinticInOutEase(time, from, to, totalTime);
                    break;
                case EaseFunction.SineInEase:
                    delta = this.SineInEase(time, from, to, totalTime);
                    break;
                case EaseFunction.SineOutEase:
                    delta = this.SineOutEase(time, from, to, totalTime);
                    break;
                case EaseFunction.SineInOutEase:
                    delta = this.SineInOutEase(time, from, to, totalTime);
                    break;
                case EaseFunction.None:
                    delta = this.LinearStep(time, from, to, totalTime);
                    break;
            }

            return delta;
        }

        #region EasingFunctions

        /// <summary>
        /// Smooth Step method
        /// </summary>
        /// <param name="time">Current time</param>
        /// <param name="value1">Start value</param>
        /// <param name="value2">End value</param>
        /// <param name="duration">Animation duration</param>
        /// <returns>delta value</returns>
        public Vector2 LinearStep(float time, Vector2 value1, Vector2 value2, float duration)
        {
            float amount = time / duration;

            Vector2 vector;
            amount = (amount > 1f) ? 1f : ((amount < 0f) ? 0f : amount);
            vector.X = value1.X + (value2.X * amount);
            vector.Y = value1.Y + (value2.Y * amount);

            return vector;
        }

        /// <summary>
        /// Back in ease function
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">End value</param>
        /// <param name="d">Animation duration</param>
        /// <returns>delta value</returns>
        public Vector2 BackInEase(float t, Vector2 b, Vector2 c, float d)
        {
            float s = 1.70158f;
            return (c * (t /= d) * t * (((s + 1) * t) - s)) + b;
        }

        /// <summary>
        /// Back out ease function
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">End value</param>
        /// <param name="d">Animation duration</param>
        /// <returns>delta value</returns>
        public Vector2 BackOutEase(float t, Vector2 b, Vector2 c, float d)
        {
            float s = 1.70158f;
            return (c * (((t = (t / d) - 1) * t * (((s + 1) * t) + s)) + 1)) + b;
        }

        /// <summary>
        /// Back in and out ease function
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">End value</param>
        /// <param name="d">Animation duration</param>
        /// <returns>delta value</returns>
        public Vector2 BackInOutEase(float t, Vector2 b, Vector2 c, float d)
        {
            float s = 1.70158f;
            if ((t /= d / 2) < 1)
            {
                return (c / 2 * (t * t * ((((s *= 1.525f) + 1) * t) - s))) + b;
            }

            return (c / 2 * (((t -= 2) * t * ((((s *= 1.525f) + 1) * t) + s)) + 2)) + b;
        }

        /// <summary>
        /// Bounce in ease function
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">End value</param>
        /// <param name="d">Animation duration</param>
        /// <returns>delta value</returns>
        public Vector2 BounceInEase(float t, Vector2 b, Vector2 c, float d)
        {
            return c - this.BounceOutEase(d - t, Vector2.Zero, c, d) + b;
        }

        /// <summary>
        /// Bounce out ease function
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">End value</param>
        /// <param name="d">Animation duration</param>
        /// <returns>delta value</returns>
        public Vector2 BounceOutEase(float t, Vector2 b, Vector2 c, float d)
        {
            if ((t /= d) < (1 / 2.75f))
            {
                return (c * (7.5625f * t * t)) + b;
            }
            else if (t < (2 / 2.75f))
            {
                return (c * ((7.5625f * (t -= 1.5f / 2.75f) * t) + .75f)) + b;
            }
            else if (t < (2.5 / 2.75))
            {
                return (c * ((7.5625f * (t -= 2.25f / 2.75f) * t) + .9375f)) + b;
            }
            else
            {
                return (c * ((7.5625f * (t -= 2.625f / 2.75f) * t) + .984375f)) + b;
            }
        }

        /// <summary>
        /// Bounce in out ease function
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">End value</param>
        /// <param name="d">Animation duration</param>
        /// <returns>delta value</returns>
        public Vector2 BounceInOutEase(float t, Vector2 b, Vector2 c, float d)
        {
            if (t < d / 2)
            {
                return (this.BounceInEase(t * 2, Vector2.Zero, c, d) * .5f) + b;
            }
            else
            {
                return (this.BounceOutEase((t * 2) - d, Vector2.Zero, c, d) * .5f) + (c * .5f) + b;
            }
        }

        /// <summary>
        /// Circle in ease function
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">End value</param>
        /// <param name="d">Animation duration</param>
        /// <returns>delta value</returns>
        public Vector2 CircleInEase(float t, Vector2 b, Vector2 c, float d)
        {
            return (-c * ((float)Math.Sqrt(1 - ((t /= d) * t)) - 1)) + b;
        }

        /// <summary>
        /// Circle out ease function
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">End value</param>
        /// <param name="d">Animation duration</param>
        /// <returns>delta value</returns>
        public Vector2 CircleOutEase(float t, Vector2 b, Vector2 c, float d)
        {
            return (c * (float)Math.Sqrt(1 - ((t = (t / d) - 1) * t))) + b;
        }

        /// <summary>
        /// Circle in and out ease function
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">End value</param>
        /// <param name="d">Animation duration</param>
        /// <returns>delta value</returns>
        public Vector2 CircleInOutEase(float t, Vector2 b, Vector2 c, float d)
        {
            if ((t /= d / 2) < 1)
            {
                return (-c / 2 * ((float)Math.Sqrt(1 - (t * t)) - 1)) + b;
            }

            return (c / 2 * ((float)Math.Sqrt(1 - ((t -= 2) * t)) + 1)) + b;
        }

        /// <summary>
        /// Cubic in ease function
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">End value</param>
        /// <param name="d">Animation duration</param>
        /// <returns>delta value</returns>
        public Vector2 CubicInEase(float t, Vector2 b, Vector2 c, float d)
        {
            return (c * (t /= d) * t * t) + b;
        }

        /// <summary>
        /// Cubic out Ease ease function
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">End value</param>
        /// <param name="d">Animation duration</param>
        /// <returns>delta value</returns>
        public Vector2 CubicOutEase(float t, Vector2 b, Vector2 c, float d)
        {
            return (c * (((t = (t / d) - 1) * t * t) + 1)) + b;
        }

        /// <summary>
        /// Cubic in and out ease function
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">End value</param>
        /// <param name="d">Animation duration</param>
        /// <returns>delta value</returns>
        public Vector2 CubicInOutEase(float t, Vector2 b, Vector2 c, float d)
        {
            if ((t /= d / 2) < 1)
            {
                return (c / 2 * t * t * t) + b;
            }

            return (c / 2 * (((t -= 2) * t * t) + 2)) + b;
        }

        /// <summary>
        /// Elastic in ease function
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">End value</param>
        /// <param name="d">Animation duration</param>
        /// <returns>delta value</returns>
        public Vector2 ElasticInEase(float t, Vector2 b, Vector2 c, float d)
        {
            if (t == 0)
            {
                return b;
            }

            if ((t /= d) == 1)
            {
                return b + c;
            }

            float p = d * .3f;
            Vector2 a = c;
            float s = p / 4;
            return -(a * (float)Math.Pow(2, 10 * (t -= 1)) * (float)Math.Sin(((t * d) - s) * (2 * (float)Math.PI) / p)) + b;
        }

        /// <summary>
        /// Elastic out ease function
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">End value</param>
        /// <param name="d">Animation duration</param>
        /// <returns>delta value</returns>
        public Vector2 ElasticOutEase(float t, Vector2 b, Vector2 c, float d)
        {
            if (t == 0)
            {
                return b;
            }

            if ((t /= d) == 1)
            {
                return b + c;
            }

            float p = d * .3f;
            Vector2 a = c;
            float s = p / 4;
            return (a * (float)Math.Pow(2, -10 * t) * (float)Math.Sin(((t * d) - s) * (2 * (float)Math.PI) / p)) + c + b;
        }

        /// <summary>
        /// Elastic in and out ease function
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">End value</param>
        /// <param name="d">Animation duration</param>
        /// <returns>delta value</returns>
        public Vector2 ElasticInOutEase(float t, Vector2 b, Vector2 c, float d)
        {
            if (t == 0)
            {
                return b;
            }

            if ((t /= d / 2) == 2)
            {
                return b + c;
            }

            float p = d * (.3f * 1.5f);
            Vector2 a = c;
            float s = p / 4;
            if (t < 1)
            {
                return (-.5f * (a * (float)Math.Pow(2, 10 * (t -= 1)) * (float)Math.Sin(((t * d) - s) * (2 * (float)Math.PI) / p))) + b;
            }

            return (a * (float)Math.Pow(2, -10 * (t -= 1)) * (float)Math.Sin(((t * d) - s) * (2 * (float)Math.PI) / p) * .5f) + c + b;
        }

        /// <summary>
        /// Exponential in ease function
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">End value</param>
        /// <param name="d">Animation duration</param>
        /// <returns>delta value</returns>
        public Vector2 ExponentialInEase(float t, Vector2 b, Vector2 c, float d)
        {
            return (t == 0) ? b : (c * (float)Math.Pow(2, 10 * ((t / d) - 1))) + b;
        }

        /// <summary>
        /// Exponential out ease function
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">End value</param>
        /// <param name="d">Animation duration</param>
        /// <returns>delta value</returns>
        public Vector2 ExponentialOutEase(float t, Vector2 b, Vector2 c, float d)
        {
            return (t == d) ? b + c : (c * (-(float)Math.Pow(2, -10 * t / d) + 1)) + b;
        }

        /// <summary>
        /// Exponential in and out ease function
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">End value</param>
        /// <param name="d">Animation duration</param>
        /// <returns>delta value</returns>
        public Vector2 ExponentialInOutEase(float t, Vector2 b, Vector2 c, float d)
        {
            if (t == 0)
            {
                return b;
            }

            if (t == d)
            {
                return b + c;
            }

            if ((t /= d / 2) < 1)
            {
                return (c / 2 * (float)Math.Pow(2, 10 * (t - 1))) + b;
            }

            return (c / 2 * (-(float)Math.Pow(2, -10 * --t) + 2)) + b;
        }

        /// <summary>
        /// Quadratic in ease function
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">End value</param>
        /// <param name="d">Animation duration</param>
        /// <returns>delta value</returns>
        public Vector2 QuadraticInEase(float t, Vector2 b, Vector2 c, float d)
        {
            return (c * (t /= d) * t) + b;
        }

        /// <summary>
        /// Quadratic out ease function
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">End value</param>
        /// <param name="d">Animation duration</param>
        /// <returns>delta value</returns>
        public Vector2 QuadraticOutEase(float t, Vector2 b, Vector2 c, float d)
        {
            return (-c * (t /= d) * (t - 2)) + b;
        }

        /// <summary>
        /// Quadratic in and out ease function
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">End value</param>
        /// <param name="d">Animation duration</param>
        /// <returns>delta value</returns>
        public Vector2 QuadraticInOutEase(float t, Vector2 b, Vector2 c, float d)
        {
            if ((t /= d / 2) < 1)
            {
                return (c / 2 * t * t) + b;
            }

            return (-c / 2 * (((--t) * (t - 2)) - 1)) + b;
        }

        /// <summary>
        /// Quartic in ease function
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">End value</param>
        /// <param name="d">Animation duration</param>
        /// <returns>delta value</returns>
        public Vector2 QuarticInEase(float t, Vector2 b, Vector2 c, float d)
        {
            return (c * (t /= d) * t * t * t) + b;
        }

        /// <summary>
        /// Quartic out ease function
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">End value</param>
        /// <param name="d">Animation duration</param>
        /// <returns>delta value</returns>
        public Vector2 QuarticOutEase(float t, Vector2 b, Vector2 c, float d)
        {
            return (-c * (((t = (t / d) - 1) * t * t * t) - 1)) + b;
        }

        /// <summary>
        /// Quartic in and out ease function
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">End value</param>
        /// <param name="d">Animation duration</param>
        /// <returns>delta value</returns>
        public Vector2 QuarticInOutEase(float t, Vector2 b, Vector2 c, float d)
        {
            if ((t /= d / 2) < 1)
            {
                return (c / 2 * t * t * t * t) + b;
            }

            return (-c / 2 * (((t -= 2) * t * t * t) - 2)) + b;
        }

        /// <summary>
        /// Quintic in ease function
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">End value</param>
        /// <param name="d">Animation duration</param>
        /// <returns>delta value</returns>
        public Vector2 QuinticInEase(float t, Vector2 b, Vector2 c, float d)
        {
            return (c * (t /= d) * t * t * t * t) + b;
        }

        /// <summary>
        /// Quintic out ease function
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">End value</param>
        /// <param name="d">Animation duration</param>
        /// <returns>delta value</returns>
        public Vector2 QuinticOutEase(float t, Vector2 b, Vector2 c, float d)
        {
            return (c * (((t = (t / d) - 1) * t * t * t * t) + 1)) + b;
        }

        /// <summary>
        /// Quintic in and out ease function
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">End value</param>
        /// <param name="d">Animation duration</param>
        /// <returns>delta value</returns>
        public Vector2 QuinticInOutEase(float t, Vector2 b, Vector2 c, float d)
        {
            if ((t /= d / 2) < 1)
            {
                return (c / 2 * t * t * t * t * t) + b;
            }

            return (c / 2 * (((t -= 2) * t * t * t * t) + 2)) + b;
        }

        /// <summary>
        /// Sine in ease function
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">End value</param>
        /// <param name="d">Animation duration</param>
        /// <returns>delta value</returns>
        public Vector2 SineInEase(float t, Vector2 b, Vector2 c, float d)
        {
            return (-c * (float)Math.Cos(t / d * (Math.PI / 2))) + c + b;
        }

        /// <summary>
        /// Sine out ease function
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">End value</param>
        /// <param name="d">Animation duration</param>
        /// <returns>delta value</returns>
        public Vector2 SineOutEase(float t, Vector2 b, Vector2 c, float d)
        {
            return (c * (float)Math.Sin(t / d * (Math.PI / 2))) + b;
        }

        /// <summary>
        /// Sine in and out ease function
        /// </summary>
        /// <param name="t">Current time</param>
        /// <param name="b">Start value</param>
        /// <param name="c">End value</param>
        /// <param name="d">Animation duration</param>
        /// <returns>delta value</returns>
        public Vector2 SineInOutEase(float t, Vector2 b, Vector2 c, float d)
        {
            return (-c / 2 * ((float)Math.Cos(Math.PI * t / d) - 1)) + b;
        }
        #endregion
    }
    #endregion
}
