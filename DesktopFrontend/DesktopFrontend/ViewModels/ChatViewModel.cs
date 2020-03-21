using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using DesktopFrontend.Models;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;

namespace DesktopFrontend.ViewModels
{
    public class ChatViewModel : ViewModelBase
    {
        public ChatViewModel(INavigationStack stack, IServerConnection connection)
        {
            ChatInit();
            ThreadSearchInit(stack, connection);
        }

        #region Chat

        public ReactiveCommand<Unit, Unit> SendMessage { get; private set; }

        private string _currentMessage = "";

        public string CurrentMessage
        {
            get => _currentMessage;
            set => this.RaiseAndSetIfChanged(ref _currentMessage, value);
        }

        private ChatMessages _messagesModel;

        public ChatMessages MessagesModel
        {
            get => _messagesModel;
            set => this.RaiseAndSetIfChanged(ref _messagesModel, value);
        }

        public ObservableCollection<ChatMessage> Messages { get; private set; }

        private void ChatInit()
        {
            MessagesModel = new ChatMessages();
            Messages = new ObservableCollection<ChatMessage>();

            var isSendEnabled = this.WhenAnyValue(
                x => x.CurrentMessage,
                x => !string.IsNullOrEmpty(x)
            );
            SendMessage = ReactiveCommand.Create(() =>
                {
                    MessagesModel.Messages.Add(new ChatMessage
                    {
                        Body = CurrentMessage,
                        Time = DateTime.Now
                    });
                    Messages.Add(MessagesModel.Messages[^1]);
                },
                isSendEnabled);
            SendMessage.Subscribe(_ => CurrentMessage = string.Empty);
        }

        #endregion

        #region Threads

        private string _threadSearch = string.Empty;

        public string ThreadSearch
        {
            get => _threadSearch;
            set => this.RaiseAndSetIfChanged(ref _threadSearch, value);
        }

        public ReactiveCommand<Unit, Unit> CreateNewThread { get; private set; }

        private ThreadSet _threadSet;
        public ObservableCollection<ThreadItem> Threads => _threadSet.Threads;

        public ReactiveCommand<Unit, Unit> UpdateThreadList { get; private set; }

        public ReactiveCommand<ThreadItem, Unit> SelectThread { get; private set; }

        private void ThreadSearchInit(INavigationStack stack, IServerConnection connection)
        {
            _threadSet = new ThreadSet();
            UpdateThreadList = ReactiveCommand.CreateFromTask(async () =>
            {
                Threads.Clear();
                var threadSet = await connection.RequestThreadSet();
                Threads.AddRange(threadSet.Threads);
            });
            UpdateThreadList.Execute();

            CreateNewThread = ReactiveCommand.Create(() =>
            {
                var createThread = new CreateNewThreadViewModel(connection);
                stack.Push(createThread);

                createThread.Create
                    .Merge(createThread.Cancel)
                    .Subscribe(_ =>
                    {
                        UpdateThreadList.Execute();
                        stack.Pop();
                    });
            });
            CreateNewThread.ThrownExceptions.Subscribe(
                e => Log.Error(Log.Areas.Network, this, e.ToString()));

            SelectThread = ReactiveCommand.CreateFromTask<ThreadItem>(async thread =>
            {
                Log.Info(Log.Areas.Application, this, $"Selected thread with name {thread.Name}");
                if (thread.Messages == null)
                {
                    thread.Messages = await connection.RequestMessagesForThread(thread);
                }

                MessagesModel = thread.Messages;
                Messages.Clear();
                Messages.AddRange(MessagesModel.Messages);
            });
        }

        #endregion
    }
}