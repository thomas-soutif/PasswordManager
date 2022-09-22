using PasswordsManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PasswordsManager.Services
{
    public static class NavigationService
    {
        private static Dictionary<object, Window> modalWindows =
            new Dictionary<object, Window>();
        private static Dictionary<object, Window> nonModalWindows =
            new Dictionary<object, Window>();
        private static Dictionary<Type, object> viewsCache =
            new Dictionary<Type, object>();
        private static Dictionary<Type, BaseViewModel> viewModelCache =
            new Dictionary<Type, BaseViewModel>();

        private static TViewModel GetViewModelInstance<TViewModel>(bool addToCache,
                                                                   params object[] viewModelParameters)
            where TViewModel : BaseViewModel
        {
            return (TViewModel)GetViewModelInstance(addToCache, typeof(TViewModel), viewModelParameters);
        }
        private static object GetViewModelInstance(bool addToCache,
                                                   Type tViewModel,
                                                   params object[] viewModelParameters)
        {
            object vm = null;
            if (addToCache && viewModelCache.ContainsKey(tViewModel))
                vm = viewModelCache[tViewModel];
            else
            {
                vm = Activator.CreateInstance(tViewModel, viewModelParameters);
                if (addToCache)
                    viewModelCache[tViewModel] = (BaseViewModel)vm;
            }
            return vm;
        }

        private static TView GetViewInstance<TView>(object viewModel)
            where TView : class
        {
            return (TView)GetViewInstance(typeof(TView), viewModel);
        }
        private static object GetViewInstance(Type tView, object viewModel)
        {
            object view = null;
            bool isWindow = tView.BaseType == typeof(Window);
            if (!isWindow && viewsCache.ContainsKey(tView))
                view = viewsCache[tView];
            else
            {
                view = Activator.CreateInstance(tView);
                var prop = tView.GetProperty("DataContext");
                prop?.SetValue(view, viewModel);
                if (!isWindow)
                    viewsCache[tView] = view;
            }
            return view;
        }

        public static TView GetPage<TView, TViewModel>(params object[] viewModelParameters)
            where TView : Page
            where TViewModel : BaseViewModel
        {
            return GetViewInstance<TView>(
                GetViewModelInstance<TViewModel>(true, viewModelParameters));
        }

        public static TView Show<TView, TViewModel>(params object[] viewModelParameters)
            where TView : Window
            where TViewModel : BaseViewModel
        {
            var vm = GetViewModelInstance<TViewModel>(false, viewModelParameters);
            var view = GetViewInstance<TView>(vm);
            nonModalWindows.Add(vm, view);
            view.Closing += (s, e) =>
            {
                var w = s as Window;
                if (w != null && nonModalWindows.ContainsKey(w.DataContext))
                    nonModalWindows.Remove(w.DataContext);
            };
            view.Show();
            return view;
        }

        public static void Close(Window view, bool? result = null)
        {
            if (modalWindows.ContainsKey(view.DataContext) && result != null)
                view.DialogResult = result;
            view.Close();
        }
        public static bool Close<TViewModel>(TViewModel vm, bool? result = null)
            where TViewModel : BaseViewModel
        {
            if (nonModalWindows.ContainsKey(vm))
            {
                Close(nonModalWindows[vm], result);
                return true;
            }
            else if (modalWindows.ContainsKey(vm))
            {
                Close(modalWindows[vm], result);
                return true;
            }
            return false;
        }

        public static bool? ShowDialog<TView, TViewModel>(params object[] viewModelParameters)
            where TView : Window
            where TViewModel : BaseViewModel
        {
            var vm = GetViewModelInstance<TViewModel>(false, viewModelParameters);
            var view = GetViewInstance<TView>(vm);
            modalWindows.Add(vm, view);
            var result = view.ShowDialog();
            if (modalWindows.ContainsKey(vm))
                modalWindows.Remove(vm);
            return result;
        }

    }
}
