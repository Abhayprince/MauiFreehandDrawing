using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiFreehandDrawing;
internal class FreehandDrawable : IDrawable
{
    #region ------------- V1 - simple circles on the touch points ------
    //private PointF _point = PointF.Zero;
    //public void SetTouchPoint(PointF point) => _point = point;

    //public void Draw(ICanvas canvas, RectF dirtyRect)
    //{
    //    if (_point.IsEmpty)
    //        return;

    //    canvas.StrokeColor = Colors.Red;
    //    canvas.StrokeSize = 10;

    //    //PointF point = new PointF(0, 0);        
    //    canvas.DrawCircle(_point, 5);
    //}

    #endregion

    #region -------------- V2 - Circles on every pointwe touch 
    //private readonly List<PointF> _points = [];
    //public void AddTouchPoint(PointF point) => _points.Add(point);
    //public void Draw(ICanvas canvas, RectF dirtyRect)
    //{
    //    if (_points.Count == 0)
    //        return;

    //    canvas.StrokeColor = Colors.Red;
    //    canvas.StrokeSize = 10;

    //    foreach (var point in _points)
    //    {
    //        canvas.DrawCircle(point, 5);
    //    }
    //}
    #endregion

    private readonly List<List<PointF>> _strokes = [];

    public void StartNewStroke(PointF point) => _strokes.Add([point]);

    public void AddTouchPoint(PointF point) => _strokes[^1].Add(point);
   
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        if (_strokes.Count == 0)
            return;

        canvas.FillColor = Colors.White;
        canvas.FillRectangle(dirtyRect);

        canvas.StrokeColor = Colors.Red;
        canvas.StrokeSize = 5;

        canvas.StrokeLineCap = LineCap.Round;
        canvas.StrokeLineJoin = LineJoin.Round;

        foreach (var points in _strokes)
        {
            if (points.Count == 0)
                continue;
            if(points.Count == 1)
            {
                // Drwa a simple point/circle
                canvas.DrawCircle(points[0], 2);                
                continue;
            }

            var path = new PathF();
            var firstPoint = points[0];
            path.MoveTo(firstPoint);

            foreach (var point in points[1..])
            {
                path.LineTo(point);
            }
            //path.Close();
            canvas.DrawPath(path);
        }
    }
}
