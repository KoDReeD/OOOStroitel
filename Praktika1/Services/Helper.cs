using Avalonia.Media;
using Microsoft.EntityFrameworkCore;
using Praktika1.Context;

namespace Praktika1.Services;

public static class Helper
{
    public static PoianwhrContext Database = new PoianwhrContext();

    public static SolidColorBrush DopColor = new SolidColorBrush(Color.FromRgb(118, 227, 131));
    public static SolidColorBrush VnimanieColor = new SolidColorBrush(Color.FromRgb(118, 227, 131));
}