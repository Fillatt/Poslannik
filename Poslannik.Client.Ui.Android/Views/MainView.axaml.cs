using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;

namespace Poslannik.Client.Ui.Android.Views
{
    public partial class MainView : UserControl
    {
        private Stack<UserControl> _navigationStack = new Stack<UserControl>();
        private UserControl _currentView;

        public MainView()
        {
            AvaloniaXamlLoader.Load(this);

            NavigateTo(new LoginView());
        }

        public void NavigateTo(UserControl view)
        {
            var contentPanel = this.FindControl<Panel>("ContentPanel");

            if (_currentView != null)
            {
                contentPanel.Children.Remove(_currentView);
            }

            _currentView = view;
            contentPanel.Children.Add(_currentView);

            ConnectNavigationHandlers(view);
        }

        public void NavigateToWithHistory(UserControl view)
        {
            var contentPanel = this.FindControl<Panel>("ContentPanel");

            if (_currentView != null)
            {
                _navigationStack.Push(_currentView);
                contentPanel.Children.Remove(_currentView);
            }

            _currentView = view;
            contentPanel.Children.Add(_currentView);

            ConnectNavigationHandlers(view);
        }

        public void NavigateBack()
        {
            if (_navigationStack.Count > 0)
            {
                var contentPanel = this.FindControl<Panel>("ContentPanel");

                if (_currentView != null)
                {
                    contentPanel.Children.Remove(_currentView);
                }

                _currentView = _navigationStack.Pop();
                contentPanel.Children.Add(_currentView);

                ConnectNavigationHandlers(_currentView);
            }
        }

        private void ConnectNavigationHandlers(UserControl view)
        {
            switch (view)
            {
                case LoginView loginView:
                    SetupLoginView(loginView);
                    break;
                case RegisterView registerView:
                    SetupRegisterView(registerView);
                    break;
                case ChatsView chatsView:
                    SetupChatsView(chatsView);
                    break;
                case ProfileView profileView:
                    SetupProfileView(profileView);
                    break;
                case ChatView chatView:
                    SetupChatView(chatView);
                    break;
                case GroupChatView groupChatView:
                    SetupGroupChatView(groupChatView);
                    break;
            }
        }

        private void SetupLoginView(LoginView view)
        {
            var loginButton = view.FindControl<Button>("LoginButton");
            var registerLink = view.FindControl<Button>("RegisterLinkButton");

            if (loginButton != null)
            {
                loginButton.Click += (s, e) => NavigateTo(new ChatsView());
            }

            if (registerLink != null)
            {
                registerLink.Click += (s, e) => NavigateTo(new RegisterView());
            }
        }

        private void SetupRegisterView(RegisterView view)
        {
            var registerButton = view.FindControl<Button>("RegisterButton");
            var loginLink = view.FindControl<Button>("LoginLinkButton");

            if (registerButton != null)
            {
                registerButton.Click += (s, e) => NavigateTo(new ChatsView());
            }

            if (loginLink != null)
            {
                loginLink.Click += (s, e) => NavigateTo(new LoginView());
            }
        }

        private void SetupChatsView(ChatsView view)
        {
            var profileTab = view.FindControl<Button>("ProfileTabButton");
            var chat1Button = view.FindControl<Button>("Chat1Button");
            var chat2Button = view.FindControl<Button>("Chat2Button");
            var chat3Button = view.FindControl<Button>("Chat3Button");
            var groupChatButton = view.FindControl<Button>("GroupChatButton");
            var chat5Button = view.FindControl<Button>("Chat5Button");

            if (profileTab != null)
            {
                profileTab.Click += (s, e) => NavigateTo(new ProfileView());
            }

            if (chat1Button != null)
            {
                chat1Button.Click += (s, e) => NavigateToWithHistory(new ChatView());
            }

            if (chat2Button != null)
            {
                chat2Button.Click += (s, e) => NavigateToWithHistory(new ChatView());
            }

            if (chat3Button != null)
            {
                chat3Button.Click += (s, e) => NavigateToWithHistory(new ChatView());
            }

            if (groupChatButton != null)
            {
                groupChatButton.Click += (s, e) => NavigateToWithHistory(new GroupChatView());
            }

            if (chat5Button != null)
            {
                chat5Button.Click += (s, e) => NavigateToWithHistory(new ChatView());
            }
        }

        private void SetupProfileView(ProfileView view)
        {
            var chatsTab = view.FindControl<Button>("ChatsTabButton");
            var logoutButton = view.FindControl<Button>("LogoutButton");

            if (chatsTab != null)
            {
                chatsTab.Click += (s, e) => NavigateTo(new ChatsView());
            }

            if (logoutButton != null)
            {
                logoutButton.Click += (s, e) =>
                {
                    _navigationStack.Clear();
                    NavigateTo(new LoginView());
                };
            }
        }

        private void SetupChatView(ChatView view)
        {
            var backButton = view.FindControl<Button>("BackButton");

            if (backButton != null)
            {
                backButton.Click += (s, e) => NavigateBack();
            }
        }

        private void SetupGroupChatView(GroupChatView view)
        {
            var backButton = view.FindControl<Button>("BackButton");

            if (backButton != null)
            {
                backButton.Click += (s, e) => NavigateBack();
            }
        }
    }
}