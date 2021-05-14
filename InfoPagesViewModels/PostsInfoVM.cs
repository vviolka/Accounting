using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using DevExpress.Mvvm;
using Model;
using Model.DBStructure;

namespace InfoPagesViewModels
{
    public class PostsInfoVM : BindableBase
    {
        private readonly PostDB dataBase;
        private readonly IErrorAlert errorAlert;

        #region selectedTabIndex

        private int selectedTabIndex;

        public int SelectedTabIndex
        {
            get
            {
                editTabVisible = selectedTabIndex == 2;
                RaisePropertyChanged(nameof(editTabVisible));
                return selectedTabIndex;

            }
            set => selectedTabIndex = value;
        }

        #endregion

        #region editTabVisible

        private bool editTabVisible;

        public bool EditTabVisible

        {
            get { return editTabVisible; }
            set { editTabVisible = value; }

        }

        #endregion


        #region AddPost

        #region AddName

        private string addName = string.Empty;

        public string AddName
        {
            set
            {
                addName = value;
            }
            get
            {
                return addName;
            }
        }
        #endregion

        #region AddRate

        private float? addRate;
        public float? AddRate
        {
            set => addRate = value;
            get => addRate;
        }


        #endregion


        #endregion


        #region SearchPost

        #region SearchName

        private string searchName = string.Empty;

        public string SearchName
        {
            get { return searchName; }
            set
            {
                nameTimer.Stop();
                nameTimer.Start();
                searchName = value;
            }
        }

        #endregion
        #region SearchRate

        private float? searchRate;
        public float? SearchRate
        {
            set
            {
                searchRate = value;
                Search();
            }
            get
            {
                return searchRate;
            }
        }
        #endregion
        #endregion


        private readonly Timer nameTimer = new Timer() { Interval = 2000 };

        private async void SearchByName(object sender, EventArgs e)
        {
            nameTimer.Stop();
            posts = dataBase.GetList(); //why?
            if (searchName != string.Empty)
                await Task.Run(() => posts = (List<Post>)dataBase.SearchByName(searchName));
            RaisePropertyChanged(nameof(posts));
        }

        //todo: make parallel

        #region SearchCancel


        private ICommand searchCancelCommand;

        public ICommand SearchCancelCommand
        {
            get => searchCancelCommand;
            set => searchCancelCommand = value;
        }

        private void SearchCancel()
        {
            searchName = string.Empty;
            searchRate = null;
            RaisePropertyChanged(nameof(searchName));
            RaisePropertyChanged(nameof(searchRate));
        }

        #endregion
        #region AddNew

        private ICommand addNewCommand;

        public ICommand AddNewCommand
        {
            get => addNewCommand;
            set => addNewCommand = value;
        }

        private void AddNew()
        {
            if (addName != string.Empty && addRate != null)
            {
                var post = new Post() { Name = addName, Rate = (float)addRate };
                dataBase.Add(post);
                Posts = dataBase.GetList();
                AddCancel();
                RaisePropertyChanged(nameof(posts));
            }
            else
                errorAlert.ErrorAlert("Форма заполнена некорректно");
        }

        #endregion

        #region LoadingIndicator

        private bool isActive = true;
        public bool IsActive
        {
            get
            {
                RaisePropertyChanged(nameof(IsEnabled));
                return isActive;

            }
            set => isActive = value;
        }

        public bool IsEnabled => !isActive;

        #endregion

        #region LoadPostsCommand

        public ICommand LoadPostsCommand
        {
            get => loadPostsCommand;
            set => loadPostsCommand = value;
        }
        private ICommand loadPostsCommand;

        public async void LoadPosts()
        {
            await Task.Run(() => Posts = dataBase.GetList());

            RaisePropertyChanged(nameof(Posts));
            IsActive = false;
            RaisePropertyChanged(nameof(isActive));
        }
        #endregion

        #region SelectedPost

        private int selectedPost;

        public int SelectedPost
        {
            get => selectedPost;
            set
            {
                selectedPost = value;
                RaisePropertyChanged(nameof(selectedPost));
            }
        }

        #endregion

        #region AddCancel

        public ICommand AddCancelCommand { get; set; }

        private void AddCancel()
        {
            addName = string.Empty;
            addRate = null;
            selectedPost = 0;
            RaisePropertyChanged(nameof(addName));
            RaisePropertyChanged(nameof(addRate));
        }

        #endregion

        #region EditPost

        #region EditName

        private string editName = string.Empty;

        public string EditName
        {
            get => editName;
            set => editName = value;
        }

        #endregion

        #region EditRate

        private float? editRate;
        public float? EditRate
        {
            set
            {
                editRate = value;
            }
            get
            {
                return editRate;
            }
        }

        #region SaveChanges

        private ICommand saveChangesCommand;
        public ICommand SaveChangesCommand
        {
            get { return saveChangesCommand; }
            set { saveChangesCommand = value; }
        }

        private void SaveChanges()
        {

            if (editName != string.Empty && editRate != null)
            {
                var post = new Post() { Name = editName, Rate = (float)editRate };
                dataBase.Edit(posts[selectedPost].Id, post);
                posts = dataBase.GetList();
                RaisePropertyChanged(nameof(posts));
                EditCancel();
            }
            else
            {
                errorAlert.ErrorAlert("Введите корректные значения для изменения");
            }
        }

        #endregion

        #region EditCancel

        private ICommand editCancelCommand;

        public ICommand EditCancelCommand
        {
            get { return editCancelCommand; }
            set { editCancelCommand = value; }
        }

        private void EditCancel()
        {
            editName = string.Empty;
            editRate = null;

            RaisePropertyChanged(nameof(editName));
            RaisePropertyChanged(nameof(editRate));

        }

        #endregion

        #region SaveAsNew

        private ICommand saveAsNewCommand;

        public ICommand SaveAsNewCommand
        {
            get { return saveAsNewCommand; }
            set { saveAsNewCommand = value; }
        }

        private void SaveAsNew()
        {
            if (editName != string.Empty && editRate != null)
            {
                var post = new Post()
                {
                    Name = editName,
                    Rate = (float)editRate,
                };
                dataBase.Add(post);
                posts = dataBase.GetList();
                RaisePropertyChanged(nameof(posts));
                EditCancel();
            }
            else
            {
                errorAlert.ErrorAlert("Введите корректные значения");
            }
        }
        #endregion

        #region Delete

        private ICommand deleteCommand;

        public ICommand DeleteCommand
        {
            get { return deleteCommand; }
            set { deleteCommand = value; }
        }

        private void Delete()
        {
            dataBase.Delete(posts[selectedPost]);
            posts = dataBase.GetList();
            RaisePropertyChanged(nameof(posts));
            EditCancel();
        }

        #endregion

        #endregion

        #endregion


        #region RowClick

        private void Search()
        {
            posts = dataBase.Search(searchName, searchRate);
            RaisePropertyChanged(nameof(posts));
        }

        public ICommand PostsRowClickCommand { get; set; }

        private void OnRowClick()
        {
            if (selectedPost == -1) return;
            selectedTabIndex = 2;
            var post = posts[selectedPost];
            editName = post.Name;
            editRate = post.Rate;

            RaisePropertyChanged(nameof(selectedPost));
            RaisePropertyChanged(nameof(selectedTabIndex));
            RaisePropertyChanged(nameof(editName));
            RaisePropertyChanged(nameof(editRate));
        }
        #endregion

        #region construcor
        public PostsInfoVM(IErrorAlert errorAlert)
        {
            saveChangesCommand = new DelegateCommand(SaveChanges);
            addNewCommand = new DelegateCommand(AddNew);
            PostsRowClickCommand = new DelegateCommand(OnRowClick);
            AddCancelCommand = new DelegateCommand(AddCancel);
            loadPostsCommand = new DelegateCommand(LoadPosts);
            nameTimer.Elapsed += SearchByName;
            editCancelCommand = new DelegateCommand(EditCancel);
            deleteCommand = new DelegateCommand(Delete);
            saveAsNewCommand = new DelegateCommand(SaveAsNew);
            searchCancelCommand = new DelegateCommand(SearchCancel);

            //post = new MaterialsM(); 
            dataBase = new PostDB();
            isActive = true;
            this.errorAlert = errorAlert;
        }
        #endregion

        private List<Post> posts;
        public List<Post> Posts
        {
            get => posts;
            set => posts = value;
        }
    }
}
