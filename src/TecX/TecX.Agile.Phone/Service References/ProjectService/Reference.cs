﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This code was auto-generated by Microsoft.Silverlight.Phone.ServiceReference, version 3.7.0.0
// 
namespace TecX.Agile.Phone.ProjectService {
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ProjectQueryResult", Namespace="http://schemas.datacontract.org/2004/07/TecX.Agile.Phone.Service")]
    public partial class ProjectQueryResult : object, System.ComponentModel.INotifyPropertyChanged {
        
        private System.Collections.ObjectModel.ObservableCollection<TecX.Agile.Phone.ProjectService.Project> ProjectsField;
        
        private int TotalResultCountField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Collections.ObjectModel.ObservableCollection<TecX.Agile.Phone.ProjectService.Project> Projects {
            get {
                return this.ProjectsField;
            }
            set {
                if ((object.ReferenceEquals(this.ProjectsField, value) != true)) {
                    this.ProjectsField = value;
                    this.RaisePropertyChanged("Projects");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int TotalResultCount {
            get {
                return this.TotalResultCountField;
            }
            set {
                if ((this.TotalResultCountField.Equals(value) != true)) {
                    this.TotalResultCountField = value;
                    this.RaisePropertyChanged("TotalResultCount");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Project", Namespace="http://schemas.datacontract.org/2004/07/TecX.Agile.Phone.Service")]
    public partial class Project : object, System.ComponentModel.INotifyPropertyChanged {
        
        private System.Guid IdField;
        
        private string NameField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="IterationQueryResult", Namespace="http://schemas.datacontract.org/2004/07/TecX.Agile.Phone.Service")]
    public partial class IterationQueryResult : object, System.ComponentModel.INotifyPropertyChanged {
        
        private System.Collections.ObjectModel.ObservableCollection<TecX.Agile.Phone.ProjectService.Iteration> IterationsField;
        
        private int TotalResultCountField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Collections.ObjectModel.ObservableCollection<TecX.Agile.Phone.ProjectService.Iteration> Iterations {
            get {
                return this.IterationsField;
            }
            set {
                if ((object.ReferenceEquals(this.IterationsField, value) != true)) {
                    this.IterationsField = value;
                    this.RaisePropertyChanged("Iterations");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int TotalResultCount {
            get {
                return this.TotalResultCountField;
            }
            set {
                if ((this.TotalResultCountField.Equals(value) != true)) {
                    this.TotalResultCountField = value;
                    this.RaisePropertyChanged("TotalResultCount");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Iteration", Namespace="http://schemas.datacontract.org/2004/07/TecX.Agile.Phone.Service")]
    public partial class Iteration : object, System.ComponentModel.INotifyPropertyChanged {
        
        private System.Guid IdField;
        
        private string NameField;
        
        private System.Guid ProjectIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid ProjectId {
            get {
                return this.ProjectIdField;
            }
            set {
                if ((this.ProjectIdField.Equals(value) != true)) {
                    this.ProjectIdField = value;
                    this.RaisePropertyChanged("ProjectId");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="StoryQueryResult", Namespace="http://schemas.datacontract.org/2004/07/TecX.Agile.Phone.Service")]
    public partial class StoryQueryResult : object, System.ComponentModel.INotifyPropertyChanged {
        
        private System.Collections.ObjectModel.ObservableCollection<TecX.Agile.Phone.ProjectService.StoryCard> StoriesField;
        
        private int TotalResultCountField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Collections.ObjectModel.ObservableCollection<TecX.Agile.Phone.ProjectService.StoryCard> Stories {
            get {
                return this.StoriesField;
            }
            set {
                if ((object.ReferenceEquals(this.StoriesField, value) != true)) {
                    this.StoriesField = value;
                    this.RaisePropertyChanged("Stories");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int TotalResultCount {
            get {
                return this.TotalResultCountField;
            }
            set {
                if ((this.TotalResultCountField.Equals(value) != true)) {
                    this.TotalResultCountField = value;
                    this.RaisePropertyChanged("TotalResultCount");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="StoryCard", Namespace="http://schemas.datacontract.org/2004/07/TecX.Agile.Phone.Service")]
    public partial class StoryCard : object, System.ComponentModel.INotifyPropertyChanged {
        
        private System.Guid IdField;
        
        private System.Guid IterationIdField;
        
        private string NameField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid IterationId {
            get {
                return this.IterationIdField;
            }
            set {
                if ((this.IterationIdField.Equals(value) != true)) {
                    this.IterationIdField = value;
                    this.RaisePropertyChanged("IterationId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://tecx.codeplex.com/phone/project", ConfigurationName="ProjectService.IProjectService")]
    public interface IProjectService {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tecx.codeplex.com/phone/project/IProjectService/GetProjects", ReplyAction="http://tecx.codeplex.com/phone/project/IProjectService/GetProjectsResponse")]
        System.IAsyncResult BeginGetProjects(int startingFromIndex, int takeCount, System.AsyncCallback callback, object asyncState);
        
        TecX.Agile.Phone.ProjectService.ProjectQueryResult EndGetProjects(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tecx.codeplex.com/phone/project/IProjectService/GetIterations", ReplyAction="http://tecx.codeplex.com/phone/project/IProjectService/GetIterationsResponse")]
        System.IAsyncResult BeginGetIterations(int startingFromIndex, int takeCount, System.Guid projectId, System.AsyncCallback callback, object asyncState);
        
        TecX.Agile.Phone.ProjectService.IterationQueryResult EndGetIterations(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tecx.codeplex.com/phone/project/IProjectService/GetUserStories", ReplyAction="http://tecx.codeplex.com/phone/project/IProjectService/GetUserStoriesResponse")]
        System.IAsyncResult BeginGetUserStories(int startingFromIndex, int takeCount, System.Guid iterationId, System.AsyncCallback callback, object asyncState);
        
        TecX.Agile.Phone.ProjectService.StoryQueryResult EndGetUserStories(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IProjectServiceChannel : TecX.Agile.Phone.ProjectService.IProjectService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetProjectsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetProjectsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public TecX.Agile.Phone.ProjectService.ProjectQueryResult Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((TecX.Agile.Phone.ProjectService.ProjectQueryResult)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetIterationsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetIterationsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public TecX.Agile.Phone.ProjectService.IterationQueryResult Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((TecX.Agile.Phone.ProjectService.IterationQueryResult)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetUserStoriesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetUserStoriesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public TecX.Agile.Phone.ProjectService.StoryQueryResult Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((TecX.Agile.Phone.ProjectService.StoryQueryResult)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ProjectServiceClient : System.ServiceModel.ClientBase<TecX.Agile.Phone.ProjectService.IProjectService>, TecX.Agile.Phone.ProjectService.IProjectService {
        
        private BeginOperationDelegate onBeginGetProjectsDelegate;
        
        private EndOperationDelegate onEndGetProjectsDelegate;
        
        private System.Threading.SendOrPostCallback onGetProjectsCompletedDelegate;
        
        private BeginOperationDelegate onBeginGetIterationsDelegate;
        
        private EndOperationDelegate onEndGetIterationsDelegate;
        
        private System.Threading.SendOrPostCallback onGetIterationsCompletedDelegate;
        
        private BeginOperationDelegate onBeginGetUserStoriesDelegate;
        
        private EndOperationDelegate onEndGetUserStoriesDelegate;
        
        private System.Threading.SendOrPostCallback onGetUserStoriesCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public ProjectServiceClient() {
        }
        
        public ProjectServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ProjectServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ProjectServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ProjectServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Net.CookieContainer CookieContainer {
            get {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    return httpCookieContainerManager.CookieContainer;
                }
                else {
                    return null;
                }
            }
            set {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    httpCookieContainerManager.CookieContainer = value;
                }
                else {
                    throw new System.InvalidOperationException("Unable to set the CookieContainer. Please make sure the binding contains an HttpC" +
                            "ookieContainerBindingElement.");
                }
            }
        }
        
        public event System.EventHandler<GetProjectsCompletedEventArgs> GetProjectsCompleted;
        
        public event System.EventHandler<GetIterationsCompletedEventArgs> GetIterationsCompleted;
        
        public event System.EventHandler<GetUserStoriesCompletedEventArgs> GetUserStoriesCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult TecX.Agile.Phone.ProjectService.IProjectService.BeginGetProjects(int startingFromIndex, int takeCount, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetProjects(startingFromIndex, takeCount, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        TecX.Agile.Phone.ProjectService.ProjectQueryResult TecX.Agile.Phone.ProjectService.IProjectService.EndGetProjects(System.IAsyncResult result) {
            return base.Channel.EndGetProjects(result);
        }
        
        private System.IAsyncResult OnBeginGetProjects(object[] inValues, System.AsyncCallback callback, object asyncState) {
            int startingFromIndex = ((int)(inValues[0]));
            int takeCount = ((int)(inValues[1]));
            return ((TecX.Agile.Phone.ProjectService.IProjectService)(this)).BeginGetProjects(startingFromIndex, takeCount, callback, asyncState);
        }
        
        private object[] OnEndGetProjects(System.IAsyncResult result) {
            TecX.Agile.Phone.ProjectService.ProjectQueryResult retVal = ((TecX.Agile.Phone.ProjectService.IProjectService)(this)).EndGetProjects(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetProjectsCompleted(object state) {
            if ((this.GetProjectsCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetProjectsCompleted(this, new GetProjectsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetProjectsAsync(int startingFromIndex, int takeCount) {
            this.GetProjectsAsync(startingFromIndex, takeCount, null);
        }
        
        public void GetProjectsAsync(int startingFromIndex, int takeCount, object userState) {
            if ((this.onBeginGetProjectsDelegate == null)) {
                this.onBeginGetProjectsDelegate = new BeginOperationDelegate(this.OnBeginGetProjects);
            }
            if ((this.onEndGetProjectsDelegate == null)) {
                this.onEndGetProjectsDelegate = new EndOperationDelegate(this.OnEndGetProjects);
            }
            if ((this.onGetProjectsCompletedDelegate == null)) {
                this.onGetProjectsCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetProjectsCompleted);
            }
            base.InvokeAsync(this.onBeginGetProjectsDelegate, new object[] {
                        startingFromIndex,
                        takeCount}, this.onEndGetProjectsDelegate, this.onGetProjectsCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult TecX.Agile.Phone.ProjectService.IProjectService.BeginGetIterations(int startingFromIndex, int takeCount, System.Guid projectId, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetIterations(startingFromIndex, takeCount, projectId, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        TecX.Agile.Phone.ProjectService.IterationQueryResult TecX.Agile.Phone.ProjectService.IProjectService.EndGetIterations(System.IAsyncResult result) {
            return base.Channel.EndGetIterations(result);
        }
        
        private System.IAsyncResult OnBeginGetIterations(object[] inValues, System.AsyncCallback callback, object asyncState) {
            int startingFromIndex = ((int)(inValues[0]));
            int takeCount = ((int)(inValues[1]));
            System.Guid projectId = ((System.Guid)(inValues[2]));
            return ((TecX.Agile.Phone.ProjectService.IProjectService)(this)).BeginGetIterations(startingFromIndex, takeCount, projectId, callback, asyncState);
        }
        
        private object[] OnEndGetIterations(System.IAsyncResult result) {
            TecX.Agile.Phone.ProjectService.IterationQueryResult retVal = ((TecX.Agile.Phone.ProjectService.IProjectService)(this)).EndGetIterations(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetIterationsCompleted(object state) {
            if ((this.GetIterationsCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetIterationsCompleted(this, new GetIterationsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetIterationsAsync(int startingFromIndex, int takeCount, System.Guid projectId) {
            this.GetIterationsAsync(startingFromIndex, takeCount, projectId, null);
        }
        
        public void GetIterationsAsync(int startingFromIndex, int takeCount, System.Guid projectId, object userState) {
            if ((this.onBeginGetIterationsDelegate == null)) {
                this.onBeginGetIterationsDelegate = new BeginOperationDelegate(this.OnBeginGetIterations);
            }
            if ((this.onEndGetIterationsDelegate == null)) {
                this.onEndGetIterationsDelegate = new EndOperationDelegate(this.OnEndGetIterations);
            }
            if ((this.onGetIterationsCompletedDelegate == null)) {
                this.onGetIterationsCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetIterationsCompleted);
            }
            base.InvokeAsync(this.onBeginGetIterationsDelegate, new object[] {
                        startingFromIndex,
                        takeCount,
                        projectId}, this.onEndGetIterationsDelegate, this.onGetIterationsCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult TecX.Agile.Phone.ProjectService.IProjectService.BeginGetUserStories(int startingFromIndex, int takeCount, System.Guid iterationId, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetUserStories(startingFromIndex, takeCount, iterationId, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        TecX.Agile.Phone.ProjectService.StoryQueryResult TecX.Agile.Phone.ProjectService.IProjectService.EndGetUserStories(System.IAsyncResult result) {
            return base.Channel.EndGetUserStories(result);
        }
        
        private System.IAsyncResult OnBeginGetUserStories(object[] inValues, System.AsyncCallback callback, object asyncState) {
            int startingFromIndex = ((int)(inValues[0]));
            int takeCount = ((int)(inValues[1]));
            System.Guid iterationId = ((System.Guid)(inValues[2]));
            return ((TecX.Agile.Phone.ProjectService.IProjectService)(this)).BeginGetUserStories(startingFromIndex, takeCount, iterationId, callback, asyncState);
        }
        
        private object[] OnEndGetUserStories(System.IAsyncResult result) {
            TecX.Agile.Phone.ProjectService.StoryQueryResult retVal = ((TecX.Agile.Phone.ProjectService.IProjectService)(this)).EndGetUserStories(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetUserStoriesCompleted(object state) {
            if ((this.GetUserStoriesCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetUserStoriesCompleted(this, new GetUserStoriesCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetUserStoriesAsync(int startingFromIndex, int takeCount, System.Guid iterationId) {
            this.GetUserStoriesAsync(startingFromIndex, takeCount, iterationId, null);
        }
        
        public void GetUserStoriesAsync(int startingFromIndex, int takeCount, System.Guid iterationId, object userState) {
            if ((this.onBeginGetUserStoriesDelegate == null)) {
                this.onBeginGetUserStoriesDelegate = new BeginOperationDelegate(this.OnBeginGetUserStories);
            }
            if ((this.onEndGetUserStoriesDelegate == null)) {
                this.onEndGetUserStoriesDelegate = new EndOperationDelegate(this.OnEndGetUserStories);
            }
            if ((this.onGetUserStoriesCompletedDelegate == null)) {
                this.onGetUserStoriesCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetUserStoriesCompleted);
            }
            base.InvokeAsync(this.onBeginGetUserStoriesDelegate, new object[] {
                        startingFromIndex,
                        takeCount,
                        iterationId}, this.onEndGetUserStoriesDelegate, this.onGetUserStoriesCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginOpen(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(callback, asyncState);
        }
        
        private object[] OnEndOpen(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndOpen(result);
            return null;
        }
        
        private void OnOpenCompleted(object state) {
            if ((this.OpenCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.OpenCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void OpenAsync() {
            this.OpenAsync(null);
        }
        
        public void OpenAsync(object userState) {
            if ((this.onBeginOpenDelegate == null)) {
                this.onBeginOpenDelegate = new BeginOperationDelegate(this.OnBeginOpen);
            }
            if ((this.onEndOpenDelegate == null)) {
                this.onEndOpenDelegate = new EndOperationDelegate(this.OnEndOpen);
            }
            if ((this.onOpenCompletedDelegate == null)) {
                this.onOpenCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnOpenCompleted);
            }
            base.InvokeAsync(this.onBeginOpenDelegate, null, this.onEndOpenDelegate, this.onOpenCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginClose(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginClose(callback, asyncState);
        }
        
        private object[] OnEndClose(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndClose(result);
            return null;
        }
        
        private void OnCloseCompleted(object state) {
            if ((this.CloseCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CloseCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void CloseAsync() {
            this.CloseAsync(null);
        }
        
        public void CloseAsync(object userState) {
            if ((this.onBeginCloseDelegate == null)) {
                this.onBeginCloseDelegate = new BeginOperationDelegate(this.OnBeginClose);
            }
            if ((this.onEndCloseDelegate == null)) {
                this.onEndCloseDelegate = new EndOperationDelegate(this.OnEndClose);
            }
            if ((this.onCloseCompletedDelegate == null)) {
                this.onCloseCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnCloseCompleted);
            }
            base.InvokeAsync(this.onBeginCloseDelegate, null, this.onEndCloseDelegate, this.onCloseCompletedDelegate, userState);
        }
        
        protected override TecX.Agile.Phone.ProjectService.IProjectService CreateChannel() {
            return new ProjectServiceClientChannel(this);
        }
        
        private class ProjectServiceClientChannel : ChannelBase<TecX.Agile.Phone.ProjectService.IProjectService>, TecX.Agile.Phone.ProjectService.IProjectService {
            
            public ProjectServiceClientChannel(System.ServiceModel.ClientBase<TecX.Agile.Phone.ProjectService.IProjectService> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginGetProjects(int startingFromIndex, int takeCount, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[2];
                _args[0] = startingFromIndex;
                _args[1] = takeCount;
                System.IAsyncResult _result = base.BeginInvoke("GetProjects", _args, callback, asyncState);
                return _result;
            }
            
            public TecX.Agile.Phone.ProjectService.ProjectQueryResult EndGetProjects(System.IAsyncResult result) {
                object[] _args = new object[0];
                TecX.Agile.Phone.ProjectService.ProjectQueryResult _result = ((TecX.Agile.Phone.ProjectService.ProjectQueryResult)(base.EndInvoke("GetProjects", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult BeginGetIterations(int startingFromIndex, int takeCount, System.Guid projectId, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[3];
                _args[0] = startingFromIndex;
                _args[1] = takeCount;
                _args[2] = projectId;
                System.IAsyncResult _result = base.BeginInvoke("GetIterations", _args, callback, asyncState);
                return _result;
            }
            
            public TecX.Agile.Phone.ProjectService.IterationQueryResult EndGetIterations(System.IAsyncResult result) {
                object[] _args = new object[0];
                TecX.Agile.Phone.ProjectService.IterationQueryResult _result = ((TecX.Agile.Phone.ProjectService.IterationQueryResult)(base.EndInvoke("GetIterations", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult BeginGetUserStories(int startingFromIndex, int takeCount, System.Guid iterationId, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[3];
                _args[0] = startingFromIndex;
                _args[1] = takeCount;
                _args[2] = iterationId;
                System.IAsyncResult _result = base.BeginInvoke("GetUserStories", _args, callback, asyncState);
                return _result;
            }
            
            public TecX.Agile.Phone.ProjectService.StoryQueryResult EndGetUserStories(System.IAsyncResult result) {
                object[] _args = new object[0];
                TecX.Agile.Phone.ProjectService.StoryQueryResult _result = ((TecX.Agile.Phone.ProjectService.StoryQueryResult)(base.EndInvoke("GetUserStories", _args, result)));
                return _result;
            }
        }
    }
}
