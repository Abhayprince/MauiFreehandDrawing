namespace MauiFreehandDrawing
{
    public partial class MainPage : ContentPage
    {
        private readonly FreehandDrawable _freehandDrawable;
        public MainPage()
        {
            InitializeComponent();

            _freehandDrawable = new FreehandDrawable();
            _graphicsView.Drawable = _freehandDrawable;
        }

        private void _graphicsView_StartInteraction(object sender, TouchEventArgs e)
        {
            var touchPoint = e.Touches[0];

            ////_freehandDrawable.SetTouchPoint(touchPoint);

            //_freehandDrawable.AddTouchPoint(touchPoint);

            //_freehandDrawable.ClearPOints();
            
            _freehandDrawable.StartNewStroke(touchPoint);
            _graphicsView.Invalidate();
        }

        private void _graphicsView_DragInteraction(object sender, TouchEventArgs e)
        {
            var touchPoint = e.Touches[0];

            _freehandDrawable.AddTouchPoint(touchPoint);

            _graphicsView.Invalidate();
        }

        private void _graphicsView_EndInteraction(object sender, TouchEventArgs e)
        {

        }

        private async void _saveBtn_Clicked(object sender, EventArgs e)
        {
            IScreenshotResult? image = await _graphicsView.CaptureAsync();
            if (image is null)
                return; // handle it
            using var imageStream = await  image.OpenReadAsync();

            var directoryPath = Path.Combine(FileSystem.AppDataDirectory, "drawings");
            Directory.CreateDirectory(directoryPath);

            var fullPath = Path.Combine(directoryPath, $"freehand_{Guid.NewGuid()}.png");

            using var fs = File.Create(fullPath);
            
            await imageStream.CopyToAsync(fs);

            _img.Source = fullPath;
            _img.IsVisible = true;
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            _img.Source = null;
            _img.IsVisible = false;
            _freehandDrawable.Reset();
            _graphicsView.Invalidate();
        }
    }

}
