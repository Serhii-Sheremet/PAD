using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace PAD
{
    public class DayView : Control
    {
        #region namespace PAD

        private TextBox editbox;
        private VScrollBar scrollbar;
        private DrawTool drawTool;
        //private SelectionTool selectionTool;
        private int allDayEventsHeaderHeight = 20;

        private DateTime workStart;
        private DateTime workEnd;

        private int firstLineY;
        private int secondLineY;
        private Pen linePen;

        #endregion

        #region Constants

        private int hourLabelWidth = 50;
        private int hourLabelIndent = 2;
        private int dayHeadersHeight = 20;
        private int appointmentGripWidth = 1; //distance between days
        private int horizontalAppointmentHeight = 20;

        private int _eventAreaWidth = 0;

        public enum AppHeightDrawMode
        {
            TrueHeightAll = 0, // all appointments have height proportional to true length
            FullHalfHourBlocksAll = 1, // all appts drawn in half hour blocks
            EndHalfHourBlocksAll = 2, // all appts boxes start at actual time, end in half hour blocks
            FullHalfHourBlocksShort = 3, // short (< 30 mins) appts drawn in half hour blocks
            EndHalfHourBlocksShort = 4, // short appts boxes start at actual time, end in half hour blocks
        }

        #endregion

        #region c.tor



        public DayView()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Selectable, true);

            scrollbar = new VScrollBar();
            scrollbar.SmallChange = halfHourHeight;
            scrollbar.LargeChange = halfHourHeight * slotsPerHour;
            scrollbar.Dock = DockStyle.Right;
            scrollbar.Visible = allowScroll;
            scrollbar.Scroll += new ScrollEventHandler(scrollbar_Scroll);
            AdjustScrollbar();
            scrollbar.Value = (startHour * slotsPerHour * halfHourHeight);

            this.Controls.Add(scrollbar);

            editbox = new TextBox();
            editbox.Multiline = true;
            editbox.Visible = false;
            editbox.ScrollBars = ScrollBars.Vertical;
            editbox.BorderStyle = BorderStyle.None;
            editbox.KeyUp += new KeyEventHandler(editbox_KeyUp);
            editbox.Margin = Padding.Empty;

            this.Controls.Add(editbox);

            drawTool = new DrawTool();
            drawTool.DayView = this;

            //selectionTool = new SelectionTool();
            //selectionTool.DayView = this;
            //selectionTool.Complete += new EventHandler(selectionTool_Complete);

            activeTool = drawTool;

            UpdateWorkingHours();

            firstLineY = 0;
            secondLineY = 0;
            linePen = new Pen(Color.Blue);

            this.Renderer = new Office12Renderer();
        }

        #endregion

        #region Properties
        private DateTime? _sunRiseDate = new DateTime().Date;
        public DateTime? SunRiseDate
        {
            get { return _sunRiseDate; }
            set { _sunRiseDate = value; }
        }


        private DateTime? _sunSetDate = new DateTime().Date;
        public DateTime? SunSetDate
        {
            get { return _sunSetDate; }
            set { _sunSetDate = value; }
        }

        private DateTime _startLineDate = new DateTime().Date;
        public DateTime StartLineDate
        {
            get { return _startLineDate; }
            set { _startLineDate = value; }
        }

        private DateTime _endLineDate = new DateTime().Date;
        public DateTime EndLineDate
        {
            get { return _endLineDate; }
            set { _endLineDate = value; }
        }

        private bool _drawFirstLine = false;
        [System.ComponentModel.DefaultValue(false)]
        public bool DrawFirstLine
        {
            get { return _drawFirstLine; }
            set { _drawFirstLine = value; }
        }

        private bool _drawSecondLine = false;
        [System.ComponentModel.DefaultValue(false)]
        public bool DrawSecondLine
        {
            get { return _drawSecondLine; }
            set { _drawSecondLine = value; }
        }

        private string _culCode = string.Empty;
        [System.ComponentModel.DefaultValue("en-US")]
        public string CultureCode
        {
            get { return _culCode; }
            set { _culCode = value; }
        }

        private string[] _arrayOfNames = new string[] { "", ""};
        public string[] ArrayOfNames
        {
            get { return _arrayOfNames; }
            set { _arrayOfNames = value; }
        }
        
        private AppHeightDrawMode appHeightMode = AppHeightDrawMode.TrueHeightAll;

        public AppHeightDrawMode AppHeightMode
        {
            get { return appHeightMode; }
            set { appHeightMode = value; }
        }

        private int halfHourHeight = 18; //?

        [System.ComponentModel.DefaultValue(18)]
        public int HalfHourHeight
        {
            get
            {
                return halfHourHeight;
            }
            set
            {
                halfHourHeight = value;
                OnHalfHourHeightChanged();
            }
        }

        private bool systemEvent = true;

        [System.ComponentModel.DefaultValue(true)]
        public bool SystemEvent
        {
            get
            {
                return systemEvent;
            }
            set
            {
                systemEvent = value;
            }
        }


        private void OnHalfHourHeightChanged()
        {
            AdjustScrollbar();
            Invalidate();
        }

        private AbstractRenderer renderer;

        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public AbstractRenderer Renderer
        {
            get
            {
                return renderer;
            }
            set
            {
                renderer = value;
                OnRendererChanged();
            }
        }

        private void OnRendererChanged()
        {
            this.Font = renderer.BaseFont;
            this.Invalidate();
        }

        private bool ampmdisplay = false;

        public bool AmPmDisplay
        {
            get
            {
                return ampmdisplay;
            }
            set
            {
                ampmdisplay = value;
                OnAmPmDisplayChanged();
            }
        }

        private void OnAmPmDisplayChanged()
        {
            this.Invalidate();
        }

        private bool drawAllAppBorder = false;

        public bool DrawAllAppBorder
        {
            get
            {
                return drawAllAppBorder;
            }
            set
            {
                drawAllAppBorder = value;
                OnDrawAllAppBorderChanged();
            }
        }

        private void OnDrawAllAppBorderChanged()
        {
            this.Invalidate();
        }

        private bool minHalfHourApp = false;

        public bool MinHalfHourApp
        {
            get
            {
                return minHalfHourApp;
            }
            set
            {
                minHalfHourApp = value;
                Invalidate();
            }
        }

        private int daysToShow = 1;  //modify to final, with value 2


        [System.ComponentModel.DefaultValue(1)]
        public int DaysToShow
        {
            get
            {
                return daysToShow;
            }
            set
            {
                daysToShow = value;
                OnDaysToShowChanged();
            }
        }

        protected virtual void OnDaysToShowChanged()
        {
            if (this.CurrentlyEditing)
                FinishEditing(true);

            Invalidate();
        }

        private SelectionType selection;

        [System.ComponentModel.Browsable(false)]
        public SelectionType Selection
        {
            get
            {
                return selection;
            }
        }

        private DateTime startDate;

        public DateTime StartDate
        {
            get
            {
                return startDate;
            }
            set
            {
                startDate = value;
                OnStartDateChanged();
            }
        }

        protected virtual void OnStartDateChanged()
        {
            startDate = startDate.Date;

            selectedAppointment = null;
            selectedAppointmentIsNew = false;
            selection = SelectionType.DateRange;

            Invalidate();
        }

        private int startHour = 8; //start of hilighted day

        [System.ComponentModel.DefaultValue(8)]
        public int StartHour
        {
            get
            {
                return startHour;
            }
            set
            {
                startHour = value;
                OnStartHourChanged();
            }
        }

        protected virtual void OnStartHourChanged()
        {
            if ((startHour * slotsPerHour * halfHourHeight) > scrollbar.Maximum) //maximum is lower on larger forms
            {
                scrollbar.Value = scrollbar.Maximum;
            }
            else
            {
                scrollbar.Value = (startHour * slotsPerHour * halfHourHeight);
            }

            Invalidate();
        }

        private Appointment selectedAppointment;

        [System.ComponentModel.Browsable(false)]
        public Appointment SelectedAppointment
        {
            get { return selectedAppointment; }
        }

        private DateTime selectionStart;

        public DateTime SelectionStart
        {
            get { return selectionStart; }
            set { selectionStart = value; }
        }

        private DateTime selectionEnd;

        public DateTime SelectionEnd
        {
            get { return selectionEnd; }
            set { selectionEnd = value; }
        }

        private ITool activeTool;

        [System.ComponentModel.Browsable(false)]
        public ITool ActiveTool
        {
            get { return activeTool; }
            set { activeTool = value; }
        }

        [System.ComponentModel.Browsable(false)]
        public bool CurrentlyEditing
        {
            get
            {
                return editbox.Visible;
            }
        }

        private int workingHourStart = 8;

        [System.ComponentModel.DefaultValue(8)]
        public int WorkingHourStart
        {
            get
            {
                return workingHourStart;
            }
            set
            {
                workingHourStart = value;
                UpdateWorkingHours();
            }
        }

        private int workingMinuteStart = 30;

        [System.ComponentModel.DefaultValue(30)]
        public int WorkingMinuteStart
        {
            get
            {
                return workingMinuteStart;
            }
            set
            {
                workingMinuteStart = value;
                UpdateWorkingHours();
            }
        }

        private int workingHourEnd = 18;

        [System.ComponentModel.DefaultValue(18)]
        public int WorkingHourEnd
        {
            get
            {
                return workingHourEnd;
            }
            set
            {
                workingHourEnd = value;
                UpdateWorkingHours();
            }
        }

        private int workingMinuteEnd = 30;  // ??

        [System.ComponentModel.DefaultValue(30)]
        public int WorkingMinuteEnd
        {
            get { return workingMinuteEnd; }
            set
            {
                workingMinuteEnd = value;
                UpdateWorkingHours();
            }
        }

        private int slotsPerHour = 4; //set amount of events per day //4 - 15 min, 6 - 10 min

        [System.ComponentModel.DefaultValue(4)]
        public int SlotsPerHour
        {
            get
            {
                return slotsPerHour;
            }
            set
            {
                slotsPerHour = value;
                Invalidate();
            }
        }

        private void UpdateWorkingHours()
        {
            workStart = new DateTime(1, 1, 1, workingHourStart, workingMinuteStart, 0);
            workEnd = new DateTime(1, 1, 1, workingHourEnd, workingMinuteEnd, 0);

            Invalidate();
        }

        bool selectedAppointmentIsNew;

        public bool SelectedAppointmentIsNew
        {
            get
            {
                return selectedAppointmentIsNew;
            }
        }

        private bool allowScroll = true;

        [System.ComponentModel.DefaultValue(true)]
        public bool AllowScroll
        {
            get
            {
                return allowScroll;
            }
            set
            {
                allowScroll = value;
                OnAllowScrollChanged();
            }
        }

        private void OnAllowScrollChanged()
        {
            this.scrollbar.Visible = this.AllowScroll;
        }

        private bool allowInplaceEditing = true;

        [System.ComponentModel.DefaultValue(true)]
        public bool AllowInplaceEditing
        {
            get
            {
                return allowInplaceEditing;
            }
            set
            {
                allowInplaceEditing = value;
            }
        }

        private bool allowNew = true;

        [System.ComponentModel.DefaultValue(true)]
        public bool AllowNew
        {
            get
            {
                return allowNew;
            }
            set
            {
                allowNew = value;
            }
        }

        #endregion

        private int HeaderHeight
        {
            get
            {
                return dayHeadersHeight + allDayEventsHeaderHeight;
            }
        }

        #region Event Handlers

        void editbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                FinishEditing(true);
            }
            else if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                FinishEditing(false);
            }
        }

        void selectionTool_Complete(object sender, EventArgs e)
        {
            if (selectedAppointment != null)
            {
                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(EnterEditMode));
            }
        }

        void scrollbar_Scroll(object sender, ScrollEventArgs e)
        {
            Invalidate();

            if (editbox.Visible)
                //scroll text box too
                editbox.Top += e.OldValue - e.NewValue;
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            base.SetBoundsCore(x, y, width, height, specified);
            AdjustScrollbar();
        }

        private void AdjustScrollbar()
        {
            scrollbar.Maximum = (slotsPerHour * halfHourHeight * 24) - this.Height + this.HeaderHeight / 2;
            scrollbar.Minimum = 0;
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // Flicker free
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            // Capture focus
            this.Focus();

            if (CurrentlyEditing)
            {
                FinishEditing(false);
            }

            if (selectedAppointmentIsNew)
            {
                RaiseNewAppointment();
            }

            ITool newTool = null;

            Appointment appointment = GetAppointmentAt(e.X, e.Y);

            if (e.Y < HeaderHeight && e.Y > dayHeadersHeight && appointment == null)
            {
                if (selectedAppointment != null)
                {
                    selectedAppointment = null;
                    Invalidate();
                }

                newTool = drawTool;
                selection = SelectionType.None;

                base.OnMouseDown(e);
                return;
            }

            if (appointment == null)
            {
                if (selectedAppointment != null)
                {
                    selectedAppointment = null;
                    Invalidate();
                }

                newTool = drawTool;
                selection = SelectionType.DateRange;
            }
            else
            {
                //newTool = selectionTool;
                selectedAppointment = appointment;
                selection = SelectionType.Appointment;

                Invalidate();
            }

            if (activeTool != null)
            {
                activeTool.MouseDown(e);
            }

            if ((activeTool != newTool) && (newTool != null))
            {
                newTool.Reset();
                newTool.MouseDown(e);
            }

            activeTool = newTool;

            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (activeTool != null)
                activeTool.MouseMove(e);

            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (activeTool != null)
                activeTool.MouseUp(e);

            base.OnMouseUp(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (e.Delta < 0)
            {//mouse wheel scroll down
                ScrollMe(true);
            }
            else
            {//mouse wheel scroll up
                ScrollMe(false);
            }
        }

        System.Collections.Hashtable cachedAppointments = new System.Collections.Hashtable();

        protected virtual void OnResolveAppointments(ResolveAppointmentsEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine("Resolve app");

            if (ResolveAppointments != null)
                ResolveAppointments(this, args);

            this.allDayEventsHeaderHeight = 0;

            // cache resolved appointments in hashtable by days.
            cachedAppointments.Clear();

            if ((selectedAppointmentIsNew) && (selectedAppointment != null))
            {
                if ((selectedAppointment.StartDate > args.StartDate) && (selectedAppointment.StartDate < args.EndDate))
                {
                    args.Appointments.Add(selectedAppointment);
                }
            }

            foreach (Appointment appointment in args.Appointments)
            {
                int key;
                AppointmentList list;

                if (appointment.StartDate.Day == appointment.EndDate.Day && appointment.AllDayEvent == false)
                {
                    key = appointment.StartDate.Day;
                }
                else
                {
                    key = -1;
                }

                list = (AppointmentList)cachedAppointments[key];

                if (list == null)
                {
                    list = new AppointmentList();
                    cachedAppointments[key] = list;
                }

                list.Add(appointment);
            }
        }

        internal void RaiseSelectionChanged(EventArgs e)
        {
            if (SelectionChanged != null)
                SelectionChanged(this, e);
        }

        internal void RaiseAppointmentMove(AppointmentEventArgs e)
        {
            if (AppointmentMove != null)
                AppointmentMove(this, e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if ((allowNew) && char.IsLetterOrDigit(e.KeyChar))
            {
                if ((this.Selection == SelectionType.DateRange))
                {
                    if (!selectedAppointmentIsNew)
                        EnterNewAppointmentMode(e.KeyChar);
                }
            }
        }

        private void EnterNewAppointmentMode(char key)
        {
            Appointment appointment = new Appointment();
            appointment.StartDate = selectionStart;
            appointment.EndDate = selectionEnd;
            appointment.Title = key.ToString();

            selectedAppointment = appointment;
            selectedAppointmentIsNew = true;

            //activeTool = selectionTool;

            Invalidate();

            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(EnterEditMode));
        }

        private delegate void StartEditModeDelegate(object state);

        private void EnterEditMode(object state)
        {
            if (!allowInplaceEditing)
                return;

            if (this.InvokeRequired)
            {
                Appointment selectedApp = selectedAppointment;

                System.Threading.Thread.Sleep(200);

                if (selectedApp == selectedAppointment)
                    this.Invoke(new StartEditModeDelegate(EnterEditMode), state);
            }
            else
            {
                StartEditing();
            }
        }

        internal void RaiseNewAppointment()
        {
            NewAppointmentEventArgs args = new NewAppointmentEventArgs(selectedAppointment.Title, selectedAppointment.StartDate, selectedAppointment.EndDate);

            if (NewAppointment != null)
            {
                NewAppointment(this, args);
            }

            selectedAppointment = null;
            selectedAppointmentIsNew = false;

            Invalidate();
        }

        #endregion

        #region Public Methods

        public void ScrollMe(bool down)
        {
            if (this.AllowScroll)
            {
                int newScrollValue;

                if (down)
                {//mouse wheel scroll down
                    newScrollValue = this.scrollbar.Value + this.scrollbar.SmallChange;

                    if (newScrollValue < this.scrollbar.Maximum)
                        this.scrollbar.Value = newScrollValue;
                    else
                        this.scrollbar.Value = this.scrollbar.Maximum;
                }
                else
                {//mouse wheel scroll up
                    newScrollValue = this.scrollbar.Value - this.scrollbar.SmallChange;

                    if (newScrollValue > this.scrollbar.Minimum)
                        this.scrollbar.Value = newScrollValue;
                    else
                        this.scrollbar.Value = this.scrollbar.Minimum;
                }

                this.Invalidate();
            }
        }

        public Rectangle GetTrueRectangle()
        {
            Rectangle truerect;

            truerect = this.ClientRectangle;
            truerect.X += hourLabelWidth + hourLabelIndent;
            truerect.Width -= scrollbar.Width + hourLabelWidth + hourLabelIndent;
            truerect.Y += this.HeaderHeight;
            truerect.Height -= this.HeaderHeight;

            truerect.X += 100;

            return truerect;
        }

        public Rectangle GetFullDayApptsRectangle()
        {
            Rectangle fulldayrect;
            fulldayrect = this.ClientRectangle;
            fulldayrect.Height = this.HeaderHeight - dayHeadersHeight;
            fulldayrect.Y += dayHeadersHeight;
            fulldayrect.Width -= (hourLabelWidth + hourLabelIndent + this.scrollbar.Width);
            fulldayrect.X += hourLabelWidth + hourLabelIndent - 100;

            return fulldayrect;
        }

        public void StartEditing()
        {
            if (!selectedAppointment.Locked && appointmentViews.ContainsKey(selectedAppointment)) //check for appointment condition
            {
                Rectangle editBounds = appointmentViews[selectedAppointment].Rectangle;

                editBounds.Inflate(-3, -3);
                editBounds.X += appointmentGripWidth - 2;
                editBounds.Width -= appointmentGripWidth - 5;

                editbox.Bounds = editBounds;
                editbox.Text = selectedAppointment.Title;
                editbox.Visible = true;
                editbox.SelectionStart = editbox.Text.Length;
                editbox.SelectionLength = 0;

                editbox.Focus();
            }
        }

        public void FinishEditing(bool cancel)
        {
            editbox.Visible = false;

            if (!cancel)
            {
                if (selectedAppointment != null)
                    selectedAppointment.Title = editbox.Text;
            }
            else
            {
                if (selectedAppointmentIsNew)
                {
                    selectedAppointment = null;
                    selectedAppointmentIsNew = false;
                }
            }

            Invalidate();
            this.Focus();
        }

        public DateTime GetTimeAt(int x, int y)
        {
            int dayWidth = (this.Width - (scrollbar.Width + hourLabelWidth + hourLabelIndent)) / daysToShow;

            int hour = (y - this.HeaderHeight + scrollbar.Value) / halfHourHeight;
            x -= hourLabelWidth;

            DateTime date = startDate;

            date = date.Date;
            date = date.AddDays(x / (dayWidth));

            if ((hour > 0) && (hour < 24 * slotsPerHour))               //??
                date = date.AddMinutes((hour * (60 / slotsPerHour)));

            return date;
        }

        public Appointment GetAppointmentAt(int x, int y)
        {
            foreach (AppointmentView view in appointmentViews.Values)
                if (view.Rectangle.Contains(x, y))
                    return view.Appointment;

            foreach (AppointmentView view in longappointmentViews.Values)
                if (view.Rectangle.Contains(x, y))
                    return view.Appointment;

            return null;
        }

        #endregion

        #region Drawing Methods

        int OnPaintCountCall = 0;

        protected override void OnPaint(PaintEventArgs e)
        {
            OnPaintCountCall++;

            if (CurrentlyEditing)
            {
                FinishEditing(false);
            }

            // resolve appointments on visible date range.
            ResolveAppointmentsEventArgs args = new ResolveAppointmentsEventArgs(this.StartDate, this.StartDate.AddDays(daysToShow));
            OnResolveAppointments(args);

            using (SolidBrush backBrush = new SolidBrush(renderer.BackColor))
                e.Graphics.FillRectangle(backBrush, this.ClientRectangle);

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Calculate visible rectangle
            Rectangle rectangle = new Rectangle(0, 0, this.Width - scrollbar.Width, this.Height);

            Rectangle daysRectangle = rectangle;
            daysRectangle.X += hourLabelWidth + hourLabelIndent;
            daysRectangle.Y += this.HeaderHeight;
            daysRectangle.Width -= (hourLabelWidth + hourLabelIndent); 

            if (e.ClipRectangle.IntersectsWith(daysRectangle))
            {
                DrawDays(e, daysRectangle);
            }

            Rectangle hourLabelRectangle = rectangle;

            hourLabelRectangle.Y += this.HeaderHeight;

            DrawHourLabels(e, hourLabelRectangle);

            Rectangle headerRectangle = rectangle;

            headerRectangle.X += hourLabelWidth + hourLabelIndent;
            headerRectangle.Width -= (hourLabelWidth + hourLabelIndent);
            headerRectangle.Height = dayHeadersHeight; //Height of a header

            if (e.ClipRectangle.IntersectsWith(headerRectangle))
            {
                DrawDayHeaders(e, headerRectangle);
            }

            Rectangle scrollrect = rectangle;

            if (this.AllowScroll == false)
            {
                scrollrect.X = headerRectangle.Width + hourLabelWidth + hourLabelIndent;
                scrollrect.Width = scrollbar.Width;
                using (SolidBrush backBrush = new SolidBrush(renderer.BackColor))
                    e.Graphics.FillRectangle(backBrush, scrollrect);
            }

            if (DrawFirstLine)
            {
                foreach (AppointmentView view in appointmentViews.Values)
                {
                    if (view.Appointment.StartDate == StartLineDate)
                    {
                        firstLineY = view.Rectangle.Y;
                        break;
                    }
                }
                int posX = hourLabelWidth + 4;
                e.Graphics.DrawLine(linePen, rectangle.Left, firstLineY, rectangle.Width, firstLineY);
                Rectangle rRect = new Rectangle(posX, firstLineY - 10, 40, 20);
                e.Graphics.DrawRoundedRectangle(linePen, rRect, 4);
                e.Graphics.FillRoundedRectangle(new SolidBrush(Color.White), rRect, 4);
                e.Graphics.DrawString(StartLineDate.ToString("HH:mm"), renderer.BaseFont, new SolidBrush(linePen.Color), posX + 4, (firstLineY - 6));
            }

            if (DrawSecondLine)
            {
                foreach (AppointmentView view in appointmentViews.Values)
                {
                    if (view.Appointment.EndDate == EndLineDate)
                    {
                        secondLineY = view.Rectangle.Y + view.Rectangle.Height;
                        break;
                    }
                }
                int posX = hourLabelWidth + 4;
                e.Graphics.DrawLine(linePen, rectangle.Left, secondLineY, rectangle.Width, secondLineY);
                Rectangle rRect = new Rectangle(posX, secondLineY - 10, 40, 20);
                e.Graphics.DrawRoundedRectangle(linePen, rRect, 4);
                e.Graphics.FillRoundedRectangle(new SolidBrush(Color.White), rRect, 4);
                e.Graphics.DrawString(EndLineDate.ToString("HH:mm"), renderer.BaseFont, new SolidBrush(linePen.Color), posX + 4, (secondLineY - 6));
            }
        }

        public void DrawA(PaintEventArgs e)
        {
            Rectangle rect = new Rectangle(100, 100, 10, 50);
            e.Graphics.SetClip(rect);
            renderer.DrawMinuteLine(e.Graphics, rect);
            Office12Renderer o12 = new Office12Renderer();
            
            e.Graphics.ResetClip();
        }

        private void DrawSunLabel(PaintEventArgs e, Rectangle sunRectangle, Rectangle rect, DateTime sunTime, bool isSunRise, int slotCount, Color color)
        {
            int posY = 0, tmpMinutes = 0;
            
            if (slotCount == 1)
                tmpMinutes = sunTime.Minute;
            else if(sunTime.Minute - ((60 / SlotsPerHour) * slotCount) == 0)
                tmpMinutes = sunTime.Minute - ((60 / SlotsPerHour) * slotCount);
            else
                tmpMinutes = sunTime.Minute - ((60 / SlotsPerHour) * (slotCount - 1));

            if (tmpMinutes > 0 && tmpMinutes != (60 / slotsPerHour))
                posY = sunRectangle.Y + ((tmpMinutes * halfHourHeight) / (60 / slotsPerHour));
            else
                posY = sunRectangle.Y + halfHourHeight - 1;
            sunRectangle.Y = posY;
            renderer.DrawMinuteLine(e.Graphics, sunRectangle.X, posY, sunRectangle.Width, color);
            renderer.DrawSunLabel(e.Graphics, sunRectangle, isSunRise, sunTime, color, this.CultureCode);
        }

        private int GetSlotCount(int sunMinutes)
        {
            int count = 0;
            double sunSlot = Convert.ToDouble(sunMinutes) / Convert.ToDouble(60 / SlotsPerHour);
            count = (int)Math.Truncate(sunSlot);
            if (count < sunSlot)
                count++;
            return count;
        }

        private void DrawHourLabels(PaintEventArgs e, Rectangle rect)
        {
            e.Graphics.SetClip(rect);
            for (int m_Hour = 0; m_Hour < 24; m_Hour++)
            {
                Rectangle hourRectangle = rect;

                hourRectangle.Y = rect.Y + (m_Hour * slotsPerHour * halfHourHeight) - scrollbar.Value;
                hourRectangle.X += hourLabelIndent;
                hourRectangle.Width = hourLabelWidth;

                renderer.DrawHourLabel(e.Graphics, hourRectangle, m_Hour, this.ampmdisplay, this.CultureCode);

                for (int slot = 0; slot < slotsPerHour; slot++)
                {
                    renderer.DrawMinuteLine(e.Graphics, hourRectangle);
                    
                    Rectangle sunRectangle = hourRectangle;
                    if (m_Hour == this.SunRiseDate.Value.Hour)
                    {
                        int slotCount = GetSlotCount(this.SunRiseDate.Value.Minute);
                        if (slot == (slotCount - 1))
                        {
                            DrawSunLabel(e, sunRectangle, rect, this.SunRiseDate.Value, true, slotCount, Color.Green);
                        }
                    }

                    if (m_Hour == this.SunSetDate.Value.Hour)
                    {
                        int slotCount = GetSlotCount(this.SunSetDate.Value.Minute);
                        if (slot == (slotCount - 1))
                        {
                            DrawSunLabel(e, sunRectangle, rect, this.SunSetDate.Value, false, slotCount, Color.Red);
                        }
                    }
                    
                    hourRectangle.Y += halfHourHeight;
                }
            }
            e.Graphics.ResetClip();
        }

        int dayFirstXEnd;
        int dayWidth;
        int rectWidth;

        private void DrawDayHeaders(PaintEventArgs e, Rectangle rect)
        {
            dayWidth = rect.Width * 10 / 100; //Header Width
            dayFirstXEnd = 0;

            for (int day = 0; day < daysToShow; day++)
            {
                if (day == 0)
                {
                    Rectangle dayHeaderRectangle = new Rectangle(rect.Left, rect.Top, dayWidth, rect.Height);
                    renderer.DrawDayHeader(e.Graphics, dayHeaderRectangle, startDate, day, string.Empty, this.CultureCode);
                    dayFirstXEnd = dayHeaderRectangle.Width;
                }

                if (day == 1)
                {
                    rectWidth = (rect.Width - dayFirstXEnd) / ArrayOfNames.Length;
                    int leftX = rect.Left + dayFirstXEnd;
                    for (int i = 0; i < ArrayOfNames.Length; i++)
                    {
                        Rectangle daySecondHeaderRectangle = new Rectangle(leftX, rect.Top, rectWidth, rect.Height);
                        renderer.DrawDayHeader(e.Graphics, daySecondHeaderRectangle, startDate, day, ArrayOfNames[i], this.CultureCode);
                        leftX += rectWidth;
                    }
                }

            }
        }

        private Rectangle GetHourRangeRectangle(DateTime start, DateTime end, Rectangle baseRectangle)
        {
            Rectangle rect = baseRectangle;

            int startY;
            int endY;

            startY = (start.Hour * halfHourHeight * slotsPerHour) + ((start.Minute * halfHourHeight) / (60 / slotsPerHour));
            endY = (end.Hour * halfHourHeight * slotsPerHour) + ((end.Minute * halfHourHeight) / (60 / slotsPerHour));

            rect.Y = startY - scrollbar.Value + this.HeaderHeight;
            rect.Height = System.Math.Max(1, (endY - startY)); 

            return rect;
        }

        DateTime _checkDate;

        private void DrawDay(PaintEventArgs e, Rectangle rect, DateTime time)
        {
            renderer.DrawDayBackground(e.Graphics, rect);

            Rectangle workingHoursRectangle = GetHourRangeRectangle(workStart, workEnd, rect);

            if (workingHoursRectangle.Y < this.HeaderHeight)
            {
                workingHoursRectangle.Height -= this.HeaderHeight - workingHoursRectangle.Y;
                workingHoursRectangle.Y = this.HeaderHeight;
            }

            renderer.DrawHourRange(e.Graphics, workingHoursRectangle, false, false);

            if ((selection == SelectionType.DateRange) && (time.Day == selectionStart.Day))
            {
                Rectangle selectionRectangle = GetHourRangeRectangle(selectionStart, selectionEnd, rect);
                if (selectionRectangle.Top + 1 > this.HeaderHeight)
                    renderer.DrawHourRange(e.Graphics, selectionRectangle, false, true);
            }

            e.Graphics.SetClip(rect);

            for (int hour = 0; hour <= 24 * slotsPerHour; hour++)
            {
                int y = rect.Top + (hour * halfHourHeight) - scrollbar.Value;

                Color color1 = renderer.HourSeperatorColor;
                Color color2 = renderer.HalfHourSeperatorColor;
                using (Pen pen = new Pen(((hour % slotsPerHour) == 0 ? color1 : color2)))
                    e.Graphics.DrawLine(pen, rect.Left, y, rect.Right, y);

                if (y > rect.Bottom)
                    break;
            }

            renderer.DrawDayGripper(e.Graphics, rect, appointmentGripWidth);

            #region second line hack
            if (time.Equals(_checkDate.AddDays(1)))
            {
                int p_X = rect.X; //save position
                rectWidth = rect.Width / ArrayOfNames.Length;
                rect.X += rectWidth; //shift position
                renderer.DrawDayGripper(e.Graphics, rect, appointmentGripWidth);
                rect.X = p_X; //return position
            }

            _checkDate = time;
            #endregion

            e.Graphics.ResetClip();

            AppointmentList appointments = (AppointmentList)cachedAppointments[time.Day];

            if (appointments != null)
            {
                List<string> groups = new List<string>();

                foreach (Appointment app in appointments)
                    if (!groups.Contains(app.Group))
                        groups.Add(app.Group);

                Rectangle rect2 = rect;
                rect2.Width = rect2.Width / groups.Count; //interaction of intersecting apoitment

                foreach (string group in groups)
                {
                    DrawAppointments(e, rect2, time, group);

                    rect2.X += rect2.Width;
                }
            }
        }

        internal Dictionary<Appointment, AppointmentView> appointmentViews = new Dictionary<Appointment, AppointmentView>();
        internal Dictionary<Appointment, AppointmentView> longappointmentViews = new Dictionary<Appointment, AppointmentView>();

        private void DrawAppointments(PaintEventArgs e, Rectangle rect, DateTime time, string group)
        {
            DateTime timeStart = time.Date;
            DateTime timeEnd = timeStart.AddHours(+24);
            timeEnd = timeEnd.AddSeconds(-1);

            AppointmentList appointments = (AppointmentList)cachedAppointments[time.Day];

            if (appointments != null)
            {
                HalfHourLayout[] layout = GetMaxParalelAppointments(appointments, this.slotsPerHour);

                List<Appointment> drawnItems = new List<Appointment>();

                for (int halfHour = 0; halfHour < 24 * slotsPerHour; halfHour++)
                {
                    HalfHourLayout hourLayout = layout[halfHour];

                    if ((hourLayout != null) && (hourLayout.Count > 0)) //hourLayout.Count - means that some apointment exist and should be drawn
                    {
                        for (int appIndex = 0; appIndex < hourLayout.Count; appIndex++)
                        {
                            Appointment appointment = hourLayout.Appointments[appIndex];

                            if (appointment.Group != group)
                                continue;  //if groups are different will just move ahead

                            if (drawnItems.IndexOf(appointment) < 0)
                            {
                                Rectangle appRect = rect;
                                int appointmentWidth;
                                AppointmentView view;

                                if (appointment.IsSystem)
                                {
                                    appointmentWidth = rect.Width;
                                }       // / appointment.conflictCount;
                                else
                                {
                                    appointmentWidth = _eventAreaWidth / appointment.conflictCount;
                                }

                                int lastX = 0;
                                foreach (Appointment app in hourLayout.Appointments)
                                {
                                    if ((app != null) && (app.Group == appointment.Group) && (appointmentViews.ContainsKey(app)))
                                    {
                                        view = appointmentViews[app];

                                        if (lastX < view.Rectangle.X)
                                            lastX = view.Rectangle.X;
                                    }
                                    
                                }

                                if ((lastX + (appointmentWidth * 2)) > (rect.X + rect.Width))
                                    lastX = 0;

                                appRect.Width = appointmentWidth;

                                if (lastX > 0)
                                    appRect.X = lastX + appointmentWidth; //indentation between appointments + 0


                                DateTime appstart = appointment.StartDate;
                                DateTime append = appointment.EndDate;

                                // Draw the appts boxes depending on the height display mode                           

                                int timeBlockCount = 0;
                                // If small appts are to be drawn in half-hour blocks
                                if (this.AppHeightMode == AppHeightDrawMode.FullHalfHourBlocksShort && appointment.EndDate.Subtract(appointment.StartDate).TotalMinutes < (60 / slotsPerHour))
                                {
                                    timeBlockCount = (appointment.EndDate.Minute / (60 / slotsPerHour));
                                    // Round the start/end time to the last/next halfhour
                                    appstart = appointment.StartDate.AddMinutes(-appointment.StartDate.Minute);
                                    append = appointment.EndDate.AddMinutes((60 / slotsPerHour));// - appointment.EndDate.Minute);

                                    if (timeBlockCount > 0)
                                    {
                                        for (int i = 0; i < timeBlockCount; i++)
                                        {
                                            if (appointment.EndDate > append && append.AddMinutes((60 / slotsPerHour)) <= appointment.EndDate)
                                                append = append.AddMinutes((60 / slotsPerHour));
                                            else
                                                append = append.AddMinutes(appointment.EndDate.Minute - (60 / slotsPerHour * timeBlockCount));
                                        }
                                    }

                                }

                                // This is basically the same as previous mode, but for all appts
                                else if (this.AppHeightMode == AppHeightDrawMode.FullHalfHourBlocksAll)
                                {
                                    timeBlockCount = 0;
                                    appstart = appointment.StartDate.AddMinutes(-appointment.StartDate.Minute);
                                    if (appointment.EndDate.Minute != 0 && appointment.EndDate.Minute != (60 / slotsPerHour) && appointment.EndDate.Minute > (60 / slotsPerHour))
                                    {
                                        timeBlockCount = (appointment.EndDate.Minute / (60 / slotsPerHour));
                                        append = appointment.EndDate.AddMinutes((60 / slotsPerHour) - appointment.EndDate.Minute);
                                    }
                                    else
                                        append = appointment.EndDate;

                                    if (timeBlockCount > 0)
                                    {
                                        for (int i = 0; i < timeBlockCount; i++)
                                        {
                                            if (appointment.EndDate > append && append.AddMinutes((60 / slotsPerHour)) <= appointment.EndDate)
                                                append = append.AddMinutes((60 / slotsPerHour));
                                            else
                                                append = append.AddMinutes(appointment.EndDate.Minute - (60 / slotsPerHour * timeBlockCount));
                                        }
                                    }

                                }

                                // Based on previous code
                                else if (this.AppHeightMode == AppHeightDrawMode.EndHalfHourBlocksShort && appointment.EndDate.Subtract(appointment.StartDate).TotalMinutes < (60 / slotsPerHour))
                                {
                                    timeBlockCount = (appointment.EndDate.Minute / (60 / slotsPerHour));
                                    // Round the end time to the next halfhour
                                    append = appointment.EndDate.AddMinutes((60 / slotsPerHour) - appointment.EndDate.Minute);

                                    // Make sure we've rounded it to the correct halfhour :)
                                    if (timeBlockCount > 0)
                                    {
                                        for (int i = 0; i < timeBlockCount; i++)
                                        {
                                            if (appointment.EndDate > append && append.AddMinutes((60 / slotsPerHour)) <= appointment.EndDate)
                                                append = append.AddMinutes((60 / slotsPerHour));
                                            else
                                                append = append.AddMinutes(appointment.EndDate.Minute - (60 / slotsPerHour * timeBlockCount));
                                        }
                                    }

                                }

                                else if (this.AppHeightMode == AppHeightDrawMode.EndHalfHourBlocksAll) //currently used
                                {
                                    timeBlockCount = 0;
                                    // Round the end time to the next halfhour
                                    if (appointment.EndDate.Minute != 0 && appointment.EndDate.Minute != (60 / slotsPerHour) && appointment.EndDate.Minute > (60 / slotsPerHour))
                                    {
                                        timeBlockCount = (appointment.EndDate.Minute / (60 / slotsPerHour));
                                        append = appointment.EndDate.AddMinutes((60 / slotsPerHour) - appointment.EndDate.Minute);
                                    }
                                    else
                                        append = appointment.EndDate;
                                    // Make sure we've rounded it to the correct halfhour :)
                                    if (timeBlockCount > 0)
                                    {
                                        for (int i = 0; i < timeBlockCount; i++)
                                        {
                                            if (appointment.EndDate > append && append.AddMinutes((60 / slotsPerHour)) <= appointment.EndDate)
                                                append = append.AddMinutes((60 / slotsPerHour));
                                            else
                                                append = append.AddMinutes(appointment.EndDate.Minute - (60 / slotsPerHour * timeBlockCount));
                                        }
                                    }
                                }

                                appRect = GetHourRangeRectangle(appstart, append, appRect);

                                view = new AppointmentView();
                                view.Rectangle = appRect;
                                view.Appointment = appointment;

                                appointmentViews[appointment] = view;

                                e.Graphics.SetClip(rect);

                                if (this.DrawAllAppBorder)
                                    appointment.DrawBorder = true;

                                // Procedure for gripper rectangle is always the same
                                Rectangle gripRect = GetHourRangeRectangle(appointment.StartDate, appointment.EndDate, appRect);
                                gripRect.Width = appointmentGripWidth;

                                renderer.DrawAppointment(e.Graphics, appRect, appointment, appointment == selectedAppointment, gripRect);

                                e.Graphics.ResetClip();

                                drawnItems.Add(appointment);
                            }
                        }
                    }
                    
                }
            }
        }

        private static HalfHourLayout[] GetMaxParalelAppointments(List<Appointment> appointments, int slotsPerHour) //add check for system events
        {
            HalfHourLayout[] appLayouts = new HalfHourLayout[24 * 50];

            foreach (Appointment appointment in appointments)
            {
                appointment.conflictCount = 1;
            }

            foreach (Appointment appointment in appointments)
            {
                int firstHalfHour = appointment.StartDate.Hour * slotsPerHour + (appointment.StartDate.Minute / (60 / slotsPerHour));
                int lastHalfHour = appointment.EndDate.Hour * slotsPerHour + (appointment.EndDate.Minute / (60 / slotsPerHour));

                // Added to allow small parts been displayed
                if (lastHalfHour == firstHalfHour)
                {
                    if (lastHalfHour < 24 * slotsPerHour)
                        lastHalfHour++;
                    else
                        firstHalfHour--;
                }

                for (int halfHour = firstHalfHour; halfHour < lastHalfHour; halfHour++)
                {
                    HalfHourLayout layout = appLayouts[halfHour];

                    if (layout == null)
                    {
                        layout = new HalfHourLayout();
                        layout.Appointments = new Appointment[50];
                        appLayouts[halfHour] = layout;
                    }

                    layout.Appointments[layout.Count] = appointment;
                    layout.Count++;

                    List<string> groups = new List<string>();

                    foreach (Appointment app2 in layout.Appointments)
                    {
                        if ((app2 != null) && (!groups.Contains(app2.Group)))
                        {
                            groups.Add(app2.Group);
                        }
                        else
                            break;
                    }

                    layout.Groups = groups;

                    // update conflicts

                    foreach (Appointment app2 in layout.Appointments)
                    {
                        if ((app2 != null) && (app2.Group == appointment.Group))
                            if (app2.conflictCount < layout.Count)
                                app2.conflictCount = layout.Count - (layout.Groups.Count - 1);
                    }

                }
            }

            return appLayouts;
        }

        private void DrawDays(PaintEventArgs e, Rectangle rect)
        {
            int dayWidth = rect.Width / daysToShow; 

            AppointmentList longAppointments = (AppointmentList)cachedAppointments[-1];
            AppointmentList drawnLongApps = new AppointmentList();
            AppointmentView view;

            int y = dayHeadersHeight;
            bool intersect = false;

            List<int> layers = new List<int>();

            if (longAppointments != null)
            {

                foreach (Appointment appointment in longAppointments)
                {
                    appointment.Layer = 0;

                    if (drawnLongApps.Count != 0)
                    {
                        foreach (Appointment app in drawnLongApps)
                            if (!layers.Contains(app.Layer))
                                layers.Add(app.Layer);

                        foreach (int lay in layers)
                        {
                            foreach (Appointment app in drawnLongApps)
                            {
                                if (app.Layer == lay)
                                    if (appointment.StartDate.Date >= app.EndDate.Date || appointment.EndDate.Date <= app.StartDate.Date)
                                        intersect = false;
                                    else
                                    {
                                        intersect = true;
                                        break;
                                    }
                                appointment.Layer = lay;
                            }

                            if (!intersect)
                                break;
                        }

                        if (intersect)
                            appointment.Layer = layers.Count;
                    }

                    drawnLongApps.Add(appointment); // changed by Gimlei
                }

                foreach (Appointment app in drawnLongApps)
                    if (!layers.Contains(app.Layer))
                        layers.Add(app.Layer);

                allDayEventsHeaderHeight = layers.Count * (horizontalAppointmentHeight + 5) + 50;

                Rectangle backRectangle = rect;
                backRectangle.Y = y;
                backRectangle.Height = allDayEventsHeaderHeight;

                renderer.DrawAllDayBackground(e.Graphics, backRectangle);

                foreach (Appointment appointment in longAppointments)
                {
                    Rectangle appointmenRect = rect;
                    int spanDays = appointment.EndDate.Subtract(appointment.StartDate).Days;

                    if (appointment.EndDate.Day != appointment.StartDate.Day && appointment.EndDate.TimeOfDay < appointment.StartDate.TimeOfDay)
                        spanDays += 1;

                    appointmenRect.Width = dayWidth * spanDays - 5;
                    appointmenRect.Height = horizontalAppointmentHeight;
                    appointmenRect.X += (appointment.StartDate.Subtract(startDate).Days) * dayWidth; // changed by Gimlei
                    appointmenRect.Y = y + appointment.Layer * (horizontalAppointmentHeight + 5) + 5; // changed by Gimlei

                    view = new AppointmentView();
                    view.Rectangle = appointmenRect;
                    view.Appointment = appointment;

                    longappointmentViews[appointment] = view;

                    Rectangle gripRect = appointmenRect;
                    gripRect.Width = appointmentGripWidth;

                    renderer.DrawAppointment(e.Graphics, appointmenRect, appointment, appointment == selectedAppointment, gripRect);


                }
            }

            DateTime time = startDate;
            Rectangle rectangle = rect;

            rectangle.Y += allDayEventsHeaderHeight;

            rectangle.Width = dayWidth; //area for events
            rectangle.Y += allDayEventsHeaderHeight;
            rectangle.Height -= allDayEventsHeaderHeight;

            appointmentViews.Clear();
            layers.Clear();

            _eventAreaWidth = rect.Width * 10 / 100;
            for (int day = 0; day < daysToShow; day++)
            {
                //change appintment area in DayView based on day number
                if (day == 0)
                    rectangle.Width += _eventAreaWidth; 
                if (day == 1)
                {
                    rectangle.X += _eventAreaWidth;
                    rectangle.Width += (rect.Width % daysToShow) + (dayWidth - 2 * _eventAreaWidth); 
                }

                DrawDay(e, rectangle, time);
                time = time.AddDays(1);
            }
        }


        public void DrawStartLine(DateTime date, Color color)
        {
            StartLineDate = date;
            linePen = new Pen(color);
            DrawFirstLine = true;
        }

        public void RemoveStartLine(DateTime date)
        {
            StartLineDate = date;
            DrawFirstLine = false;
        }

        public void DrawEndLine(DateTime date, Color color)
        {
            EndLineDate = date;
            linePen = new Pen(color);
            DrawSecondLine = true;
        }

        public void RemoveEndLine(DateTime date)
        {
            EndLineDate = date;
            DrawSecondLine = false;
        }


        #endregion

        #region Internal Utility Classes

        class HalfHourLayout
        {
            public int Count;
            public List<string> Groups;
            public Appointment[] Appointments;
        }

        internal class AppointmentView
        {
            public Appointment Appointment;
            public Rectangle Rectangle;
        }

        class AppointmentList : List<Appointment>
        {
        }

        #endregion

        #region Events

        public event EventHandler SelectionChanged;
        public event ResolveAppointmentsEventHandler ResolveAppointments;
        public event NewAppointmentEventHandler NewAppointment;
        public event EventHandler<AppointmentEventArgs> AppointmentMove;

        #endregion
    }
}
