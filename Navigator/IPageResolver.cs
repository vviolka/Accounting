using System.Windows.Controls;

namespace Navigator
{
    public interface IPageResolver
    {
        Page GetPageInstance(string alias);
    }
}
