﻿// Copyright © 2018 Wave Engine S.L. All rights reserved. Use is subject to license terms.

#region Using Statements
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using WaveEngine.Common.Graphics;
using WaveEngine.Components.Gestures;
using WaveEngine.Framework;
using WaveEngine.Framework.Animation;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Framework.UI;
#endregion

namespace WaveEngine.Components.UI
{
    /// <summary>
    /// UI Slider decorate class
    /// </summary>
    public class Slider : UIBase
    {
        /// <summary>
        /// The instances
        /// </summary>
        private static int instances;

        /// <summary>
        /// Occurs when [value changed].
        /// </summary>
        public event ChangedEventHandler ValueChanged;

        /// <summary>
        /// Occurs when [real time value changed].
        /// </summary>
        public event ChangedEventHandler RealTimeValueChanged;

        #region Properties

        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        /// <value>
        /// The maximum.
        /// </value>
        [DataMember]
        public int Maximum
        {
            get
            {
                return this.entity.FindComponent<SliderBehavior>().Maximum;
            }

            set
            {
                this.entity.FindComponent<SliderBehavior>().Maximum = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        /// <value>
        /// The minimum.
        /// </value>
        [DataMember]
        public int Minimum
        {
            get
            {
                return this.entity.FindComponent<SliderBehavior>().Minimum;
            }

            set
            {
                this.entity.FindComponent<SliderBehavior>().Minimum = value;
            }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [DataMember]
        public int Value
        {
            get
            {
                return this.entity.FindComponent<SliderBehavior>().Value;
            }

            set
            {
                this.entity.FindComponent<SliderBehavior>().Value = value;
            }
        }

        /// <summary>
        /// Gets or sets the orientation.
        /// </summary>
        /// <value>
        /// The orientation.
        /// </value>
        [DataMember]
        public Orientation Orientation
        {
            get
            {
                return this.entity.FindComponent<SliderBehavior>().Orientation;
            }

            set
            {
                this.entity.FindComponent<SliderBehavior>().Orientation = value;
            }
        }

        /// <summary>
        /// Gets or sets the margin.
        /// </summary>
        /// <value>
        /// The margin.
        /// </value>
        [DataMember]
        public Thickness Margin
        {
            get
            {
                return this.entity.FindComponent<PanelControl>().Margin;
            }

            set
            {
                this.entity.FindComponent<PanelControl>().Margin = value;
            }
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        [DataMember]
        public float Width
        {
            get
            {
                return this.entity.FindComponent<PanelControl>().Width;
            }

            set
            {
                this.entity.FindComponent<PanelControl>().Width = value;
                this.entity.FindComponent<SliderBehavior>().UpdateWidth = value;
            }
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        [DataMember]
        public float Height
        {
            get
            {
                return this.entity.FindComponent<PanelControl>().Height;
            }

            set
            {
                this.entity.FindComponent<PanelControl>().Height = value;
                this.entity.FindComponent<SliderBehavior>().UpdateHeight = value;
            }
        }

        /// <summary>
        /// Gets or sets the horizontal alignment.
        /// </summary>
        /// <value>
        /// The horizontal alignment.
        /// </value>
        [DataMember]
        public HorizontalAlignment HorizontalAlignment
        {
            get
            {
                return this.entity.FindComponent<PanelControl>().HorizontalAlignment;
            }

            set
            {
                this.entity.FindComponent<PanelControl>().HorizontalAlignment = value;
            }
        }

        /// <summary>
        /// Gets or sets the vertical alignment.
        /// </summary>
        /// <value>
        /// The vertical alignment.
        /// </value>
        [DataMember]
        public VerticalAlignment VerticalAlignment
        {
            get
            {
                return this.entity.FindComponent<PanelControl>().VerticalAlignment;
            }

            set
            {
                this.entity.FindComponent<PanelControl>().VerticalAlignment = value;
            }
        }

        /// <summary>
        /// Gets or sets the foreground.
        /// </summary>
        /// <value>
        /// The foreground.
        /// </value>
        [DataMember]
        public Color Foreground
        {
            get
            {
                return this.entity.FindChild("ForegroundEntity").FindComponent<ImageControl>().TintColor;
            }

            set
            {
                this.entity.FindChild("ForegroundEntity").FindComponent<ImageControl>().TintColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the background.
        /// </summary>
        /// <value>
        /// The background.
        /// </value>
        [DataMember]
        public Color Background
        {
            get
            {
                return this.entity.FindChild("BackgroundEntity").FindComponent<ImageControl>().TintColor;
            }

            set
            {
                this.entity.FindChild("BackgroundEntity").FindComponent<ImageControl>().TintColor = value;
            }
        }

        /// <summary>
        /// Sets the font.
        /// </summary>
        /// <value>
        /// The font.
        /// </value>
        public string FontPath
        {
            set
            {
                Entity textEntity = this.entity.FindChild("TextEntity");
                TextControl textBlock = textEntity.FindComponent<TextControl>();
                textEntity.RemoveComponent<TextControl>();
                textEntity.AddComponent(new TextControl(value)
                {
                    Text = textBlock.Text,
                    Foreground = textBlock.Foreground,
                    Margin = textBlock.Margin,
                    HorizontalAlignment = textBlock.HorizontalAlignment,
                    VerticalAlignment = textBlock.VerticalAlignment,
                    LineSpacing = textBlock.LineSpacing,
                    LineWidth = textBlock.LineWidth,
                    TouchMargin = textBlock.TouchMargin,
                    TextWrapping = textBlock.TextWrapping
                });

                textEntity.RefreshDependencies();
            }
        }

        /// <summary>
        /// Gets or sets the foreground.
        /// </summary>
        /// <value>
        /// The foreground.
        /// </value>
        [DataMember]
        public Color TextColor
        {
            get
            {
                return this.entity.FindChild("TextEntity").FindComponent<TextControl>().Foreground;
            }

            set
            {
                this.entity.FindChild("TextEntity").FindComponent<TextControl>().Foreground = value;
            }
        }
        #endregion

        #region Initialize

        /// <summary>
        /// Initializes a new instance of the <see cref="Slider" /> class.
        /// </summary>
        public Slider()
            : this("Slider" + instances++)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Slider" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public Slider(string name)
        {
            this.entity = new Entity(name)
                           .AddComponent(new Transform2D())
                           .AddComponent(new RectangleCollider2D())
                           .AddComponent(new TouchGestures(false))
                           .AddComponent(new PanelControl(100, 20))
                           .AddComponent(new PanelControlRenderer())
                           .AddComponent(new SliderBehavior())
                           .AddChild(new Entity("BackgroundEntity")
                                .AddComponent(new Transform2D()
                                {
                                    DrawOrder = 0.5f
                                })
                                .AddComponent(new ImageControl(Color.Blue, 1, 1))
                                .AddComponent(new ImageControlRenderer()))
                            .AddChild(new Entity("ForegroundEntity")
                                .AddComponent(new Transform2D()
                                {
                                    DrawOrder = 0.45f
                                })
                                .AddComponent(new ImageControl(Color.LightBlue, 1, 1))
                                .AddComponent(new ImageControlRenderer()))
                            .AddChild(new Entity("BulletEntity")
                                .AddComponent(new Transform2D()
                                {
                                    DrawOrder = 0.4f
                                })
                                .AddComponent(new ImageControl(Color.White, 1, 1))
                                .AddComponent(new ImageControlRenderer()))
                            .AddChild(new Entity("TextEntity")
                                .AddComponent(new Transform2D()
                                {
                                    DrawOrder = 0.4f,
                                    Opacity = 0
                                })
                                .AddComponent(new AnimationUI())
                                .AddComponent(new TextControl())
                                .AddComponent(new TextControlRenderer()));

            // Event
            this.entity.FindComponent<SliderBehavior>().ValueChanged += this.Slider_ValueChanged;
            this.entity.FindComponent<SliderBehavior>().RealTimeValueChanged += this.Slider_RealTimeValueChanged;
        }

        #endregion

        #region Public Methods
        #endregion

        #region Private Methods

        /// <summary>
        /// Handles the ValueChanged event of the Slider control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void Slider_ValueChanged(object sender, ChangedEventArgs e)
        {
            if (this.ValueChanged != null)
            {
                this.ValueChanged(this, e);
            }
        }

        /// <summary>
        /// Handles the RealTimeValueChanged event of the Slider control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ChangedEventArgs" /> instance containing the event data.</param>
        private void Slider_RealTimeValueChanged(object sender, ChangedEventArgs e)
        {
            if (this.RealTimeValueChanged != null)
            {
                this.RealTimeValueChanged(this, e);
            }
        }
        #endregion
    }
}
