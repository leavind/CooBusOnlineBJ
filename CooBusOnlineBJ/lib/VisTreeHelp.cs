using System.Windows.Media;
using System.Windows;
using System.Collections.Generic;

public class VisTreeHelp
{
    public static childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(obj, i);
            if (child != null && child is childItem)
                return (childItem)child;
            else
            {
                childItem childOfChild = FindVisualChild<childItem>(child);
                if (childOfChild != null)
                    return childOfChild;
            }
        }
        return null;
    }

    public static List<T> GetChildObjects<T>(DependencyObject obj, string name) where T : FrameworkElement
    {
        DependencyObject child = null;
        List<T> childList = new List<T>();
        for (int i = 0; i <VisualTreeHelper.GetChildrenCount(obj); i++)
        {
            child = VisualTreeHelper.GetChild(obj, i);
            if (child is T && (((T)child).Name == name || string.IsNullOrEmpty(name)))
                childList.Add((T)child);
            childList.AddRange(GetChildObjects<T>(child, ""));
        }
        return childList;
    }


    public static List<T> GetChildObjects<T>(DependencyObject obj) where T : FrameworkElement
    {
        DependencyObject child = null;
        List<T> childList = new List<T>();
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
        {
            child = VisualTreeHelper.GetChild(obj, i);
            if (child is T)
                childList.Add((T)child);
            childList.AddRange(GetChildObjects<T>(child));
        }
        return childList;
    }
}