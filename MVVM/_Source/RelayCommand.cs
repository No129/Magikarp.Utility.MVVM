using System;
using System.Windows.Input;

namespace Magikarp.Utility.MVVM
{
    /// <summary>
    /// 中繼命令物件。
    /// 參考: https://www.codeproject.com/Tips/813345/Basic-MVVM-and-ICommand-Usage-Example
    /// </summary>
    /// <remarks>
    /// Author: 黃竣祥
    /// Version: 20170926
    /// </remarks>
    public class RelayCommand : ICommand
    {

        #region -- 變數宣告 ( Declarations ) --   

        private Action<object> m_objExecuteDelegate;
        private Predicate<object> m_objCanExecuteDelegate;

        #endregion

        #region -- 建構/解構 ( Constructors/Destructor ) --

        /// <summary>
        /// 建構元。
        /// </summary>
        /// <param name="pi_objExecuteDelegate">執行命令委派。</param>
        /// <remarks>
        /// Author: 黃竣祥
        /// Time: 2017/09/26
        /// History: N/A
        /// DB Object: N/A      
        /// </remarks>
        public RelayCommand(Action<object> pi_objExecuteDelegate) : this(pi_objExecuteDelegate, DefaultCanExecute) { }

        /// <summary>
        /// 建構元。
        /// </summary>
        /// <param name="pi_objExecuteDelegate">執行命令委派。</param>
        /// <param name="pi_objCanExecuteDelegate">是否可供執行委派。</param>
        /// <remarks>
        /// Author: 黃竣祥
        /// Time: 2017/09/26
        /// History: N/A
        /// DB Object: N/A      
        /// </remarks>
        public RelayCommand(Action<object> pi_objExecuteDelegate, Predicate<object> pi_objCanExecuteDelegate)
        {
            if (pi_objExecuteDelegate == null)
            {
                throw new ArgumentNullException("execute");
            }

            if (pi_objCanExecuteDelegate == null)
            {
                throw new ArgumentNullException("canExecute");
            }

            this.m_objExecuteDelegate = pi_objExecuteDelegate;
            this.m_objCanExecuteDelegate = pi_objCanExecuteDelegate;
        }

        #endregion

        #region -- 事件處理 ( Event Handlers ) --

        /// <summary>
        /// 處理「是否可供執行」判斷異動事件。
        /// </summary>
        /// <remarks>
        /// Author: 黃竣祥
        /// Time: 2017/09/26
        /// History: N/A
        /// DB Object: N/A      
        /// </remarks>
        public void OnCanExecuteChanged()
        {
            EventHandler handler = this.CanExecuteChangedInternal;
            if (handler != null)
            {
                //DispatcherHelper.BeginInvokeOnUIThread(() => handler.Invoke(this, EventArgs.Empty));
                handler.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion

        #region -- 事件 ( Events ) --

        /// <summary>
        /// 是否可供執行異動事件。
        /// </summary>
        /// <remarks>
        /// Author: 黃竣祥
        /// Time: 2017/09/26
        /// History: N/A
        /// DB Object: N/A      
        /// </remarks>
        private event EventHandler CanExecuteChangedInternal;

        #endregion

        #region -- 介面實做 ( Implements ) - [ICommand] --

        /// <summary>
        /// 當 Command 是否可執行的狀態改變後引發的事件。
        /// </summary>
        /// <remarks>
        /// Author: 黃竣祥
        /// Time: 2017/09/26
        /// History: N/A
        /// DB Object: N/A      
        /// </remarks>
        event EventHandler ICommand.CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                this.CanExecuteChangedInternal += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
                this.CanExecuteChangedInternal -= value;
            }
        }

        /// <summary>
        /// 決定 Command 是否開放執行。
        /// </summary>
        /// <param name="parameter">判斷參數。</param>
        /// <returns>是否開放執行。</returns>
        /// <remarks>
        /// Author: 黃竣祥
        /// Time: 2017/09/26
        /// History: N/A
        /// DB Object: N/A      
        /// </remarks>
        bool ICommand.CanExecute(object parameter)
        {
            return this.m_objCanExecuteDelegate != null && this.m_objCanExecuteDelegate(parameter);
        }

        /// <summary>
        /// 執行 Command 對應的動作。
        /// </summary>
        /// <param name="parameter">判斷參數。</param>
        /// <remarks>
        /// Author: 黃竣祥
        /// Time: 2017/09/26
        /// History: N/A
        /// DB Object: N/A      
        /// </remarks>
        void ICommand.Execute(object parameter)
        {
            this.m_objExecuteDelegate(parameter);
        }

        #endregion

        #region -- 私有函式 ( Private Method) --

        /// <summary>
        /// 預設「是否可執行」邏輯。
        /// </summary>
        /// <param name="parameter">判斷參數。</param>
        /// <returns>是否可執行。</returns>
        /// <remarks>
        /// Author: 黃竣祥
        /// Time: 2017/09/26
        /// History: N/A
        /// DB Object: N/A      
        /// </remarks>
        private static bool DefaultCanExecute(object parameter)
        {
            return true;
        }

        #endregion        
    }
}
