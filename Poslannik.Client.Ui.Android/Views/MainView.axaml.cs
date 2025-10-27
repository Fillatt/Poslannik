using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using System.Collections.Generic;
using System.Linq;

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
                case NewChatView newChatView:
                    SetupNewChatView(newChatView);
                    break;
                case UserProfileView userProfileView:
                    SetupUserProfileView(userProfileView);
                    break;
                case ParticipantsView participantsView:
                    SetupParticipantsView(participantsView);
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
            var addChatButton = view.FindControl<Button>("AddChatButton");
            var chat1Button = view.FindControl<Button>("Chat1Button");
            var chat2Button = view.FindControl<Button>("Chat2Button");
            var chat3Button = view.FindControl<Button>("Chat3Button");
            var groupChatButton = view.FindControl<Button>("GroupChatButton");
            var chat5Button = view.FindControl<Button>("Chat5Button");

            if (profileTab != null)
            {
                profileTab.Click += (s, e) => NavigateTo(new ProfileView());
            }

            if (addChatButton != null)
            {
                addChatButton.Click += (s, e) => NavigateToWithHistory(new NewChatView());
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
            var userAvatarButton = view.FindControl<Border>("UserAvatarBorder");

            if (backButton != null)
            {
                backButton.Click += (s, e) => NavigateBack();
            }

            if (userAvatarButton == null)
            {
                var borders = view.GetVisualDescendants().OfType<Border>();
                foreach (var border in borders)
                {
                    if (border.Width == 35 && border.Height == 35 &&
                        border.CornerRadius == new Avalonia.CornerRadius(17.5))
                    {
                        userAvatarButton = border;
                        break;
                    }
                }
            }

            if (userAvatarButton != null)
            {
                userAvatarButton.PointerPressed += (s, e) => NavigateToWithHistory(new UserProfileView());
            }
        }

        private void SetupGroupChatView(GroupChatView view)
        {
            var backButton = view.FindControl<Button>("BackButton");
            var groupInfoButton = view.FindControl<Button>("GroupInfoButton");

            if (backButton != null)
            {
                backButton.Click += (s, e) => NavigateBack();
            }

            if (groupInfoButton != null)
            {
                groupInfoButton.Click += (s, e) => NavigateToWithHistory(new ParticipantsView());
            }
        }

        private void SetupNewChatView(NewChatView view)
        {
            var backButton = view.FindControl<Button>("BackButton");
            var chatsTabButton = view.FindControl<Button>("ChatsTabButton");
            var profileTabButton = view.FindControl<Button>("ProfileTabButton");
            var createChatButton = view.FindControl<Button>("CreateChatButton");

            if (backButton != null)
            {
                backButton.Click += (s, e) => NavigateBack();
            }

            if (chatsTabButton != null)
            {
                chatsTabButton.Click += (s, e) => NavigateTo(new ChatsView());
            }

            if (profileTabButton != null)
            {
                profileTabButton.Click += (s, e) => NavigateTo(new ProfileView());
            }

            if (createChatButton != null)
            {
                createChatButton.Click += (s, e) =>
                {
                    _navigationStack.Clear();
                    NavigateTo(new ChatsView());
                };
            }
        }

        private void SetupUserProfileView(UserProfileView view)
        {
            var backButton = view.FindControl<Button>("BackButton");
            var chatsTabButton = view.FindControl<Button>("ChatsTabButton");
            var profileTabButton = view.FindControl<Button>("ProfileTabButton");

            if (backButton != null)
            {
                backButton.Click += (s, e) => NavigateBack();
            }

            if (chatsTabButton != null)
            {
                chatsTabButton.Click += (s, e) => NavigateTo(new ChatsView());
            }

            if (profileTabButton != null)
            {
                profileTabButton.Click += (s, e) => NavigateTo(new ProfileView());
            }
        }

        private void SetupParticipantsView(ParticipantsView view)
        {
            var backButton = view.FindControl<Button>("BackButton");
            var chatsTabButton = view.FindControl<Button>("ChatsTabButton");
            var profileTabButton = view.FindControl<Button>("ProfileTabButton");
            var addParticipantButton = view.FindControl<Button>("AddParticipantButton");
            var deleteChatButton = view.FindControl<Button>("DeleteChatButton");
            var leaveChatButton = view.FindControl<Button>("LeaveChatButton");
            var myProfileButton = view.FindControl<Button>("MyProfileButton");
            var participant1Button = view.FindControl<Button>("Participant1Button");
            var participant2Button = view.FindControl<Button>("Participant2Button");

            if (backButton != null)
            {
                backButton.Click += (s, e) => NavigateBack();
            }

            if (chatsTabButton != null)
            {
                chatsTabButton.Click += (s, e) => NavigateTo(new ChatsView());
            }

            if (profileTabButton != null)
            {
                profileTabButton.Click += (s, e) => NavigateTo(new ProfileView());
            }

            if (addParticipantButton != null)
            {
                addParticipantButton.Click += (s, e) => NavigateToWithHistory(new NewChatView());
            }

            if (deleteChatButton != null)
            {
                deleteChatButton.Click += (s, e) =>
                {
                    _navigationStack.Clear();
                    NavigateTo(new ChatsView());
                };
            }

            if (leaveChatButton != null)
            {
                leaveChatButton.Click += (s, e) =>
                {
                    _navigationStack.Clear();
                    NavigateTo(new ChatsView());
                };
            }

            if (myProfileButton != null)
            {
                myProfileButton.Click += (s, e) => NavigateToWithHistory(new UserProfileView());
            }

            if (participant1Button != null)
            {
                participant1Button.Click += (s, e) => NavigateToWithHistory(new UserProfileView());
            }

            if (participant2Button != null)
            {
                participant2Button.Click += (s, e) => NavigateToWithHistory(new UserProfileView());
            }
        }
    }
}