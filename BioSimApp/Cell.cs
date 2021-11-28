using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using BioSimLib;
using BioSimLib.Actions;
using BioSimLib.Field;
using BioSimLib.Sensors;

namespace BioSimApp;

public class Cell
{
    private readonly Player _player;
    private readonly Brush _brush;

    public Cell(Player player)
    {
        _player = player;

        var c = _player.Color;
        var color = Color.FromRgb(c.Item1, c.Item2, c.Item3);
        _brush = new SolidColorBrush(color);
    }

    public void Draw(Canvas myCanvas, double scaleFactor)
    {
        var tx = 2.0 + _player._loc.X * scaleFactor;
        var ty = 2.0 + _player._loc.Y * scaleFactor;
        var radius = 0.5 * scaleFactor;
        var center = new Point(tx, ty);
        var circle = new EllipseGeometry(center, radius, radius, null);
        var path = new Path
        {
            Fill = _brush,
            Stroke = _brush,
            StrokeThickness = 0.1,
            Data = circle,
        };
            
        myCanvas.Children.Add(path);
    }

    static bool IsEnabled(IAction action) => (int)action.Type < (int)Action.KILL_FORWARD;

    public void Update(Board board, SensorFactory sensorsFactory, ActionFactory actionFactory, float[] actionLevels, float[] neuronAccumlator, uint simStep)
    {
        for (var i = 0; i < actionLevels.Length; i++) actionLevels[i] = 0.0f;
        for (var i = 0; i < neuronAccumlator.Length; i++) neuronAccumlator[i] = 0.0f;

        _player.FeedForward(sensorsFactory, actionLevels, neuronAccumlator, simStep);
        _player.ExecuteActions(actionFactory, board, IsEnabled, actionLevels, simStep);
        var newLoc = _player.ExecuteMoves(actionFactory, IsEnabled, actionLevels, simStep);
        if (board.Grid.IsInBounds(newLoc) && board.Grid.IsEmptyAt(newLoc))
            board.Peeps.QueueForMove(_player, newLoc);
    }
}