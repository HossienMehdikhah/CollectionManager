﻿using System.Diagnostics.CodeAnalysis;
using CollectionManager.WinUI.Contracts;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using WinUI;
using CommunityToolkit.WinUI.UI.Animations;
namespace CollectionManager.WinUI.Services;

public class NavigationService(IPageService pageService) : INavigationService
{
    private readonly IPageService _pageService = pageService;
    private object? _lastParameterUsed;
    private Frame? _frame;

    public event NavigatedEventHandler? Navigated;
    public Frame? Frame
    {
        get
        {
            if (_frame == null)
            {
                _frame = App.MainWindow.Content as Frame;
                RegisterFrameEvents();
            }

            return _frame;
        }

        set
        {
            UnregisterFrameEvents();
            _frame = value;
            RegisterFrameEvents();
        }
    }
    [MemberNotNullWhen(true, nameof(Frame), nameof(_frame))]
    public bool CanGoBack => Frame != null && Frame.CanGoBack;
    public bool NavigateTo(string pageKey, object? parameter = null, bool clearNavigation = false)
    {
        var pageType = _pageService.GetPageType(pageKey);
        if (_frame is null)
            return false;

        if (_frame.Content?.GetType() != pageType || (parameter != null && !parameter.Equals(_lastParameterUsed)))
        {
            _frame.Tag = clearNavigation;
            var vmBeforeNavigation = GetPageViewModel(_frame);
            var navigated = _frame.Navigate(pageType, parameter);
            if (navigated)
            {
                _lastParameterUsed = parameter;
                if (vmBeforeNavigation is INavigationAware navigationAware)
                    navigationAware.OnNavigatedFrom();
            }

            return navigated;
        }

        return false;
    }
    public bool GoBack()
    {
        if (!CanGoBack)
            return false;

        var vmBeforeNavigation = GetPageViewModel(_frame);
        _frame.GoBack();
        if (vmBeforeNavigation is INavigationAware navigationAware)
            navigationAware.OnNavigatedFrom();
        return true;
    }
    public void SetListDataItemForNextConnectedAnimation(object item) => Frame.SetListDataItemForNextConnectedAnimation(item);

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        if (sender is not Frame frame)
            return;

        var clearNavigation = (bool)frame.Tag;
        if (clearNavigation)
            frame.BackStack.Clear();
        if (GetPageViewModel(frame) is INavigationAware navigationAware)
            navigationAware.OnNavigatedTo(e.Parameter);
        Navigated?.Invoke(sender, e);
    }
    private static object? GetPageViewModel(Frame frame)
    {
        return frame?.Content?.GetType().GetProperty("ViewModel")?.GetValue(frame.Content, null);
    }
    private void RegisterFrameEvents()
    {
        if (_frame != null)
        {
            _frame.Navigated += OnNavigated;
        }
    }
    private void UnregisterFrameEvents()
    {
        if (_frame != null)
        {
            _frame.Navigated -= OnNavigated;
        }
    }
}
