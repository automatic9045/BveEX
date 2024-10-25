using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Diagnostics
{
    /// <summary>
    /// 例外をラップしている <see cref="AggregateException"/> などの例外を一元的に解決するための機能を提供します。
    /// </summary>
    /// <remarks>
    /// 異なるバージョン間において、例外の展開方法が一意である保証はありませんので注意してください。
    /// </remarks>
    public class WrapperExceptionExtractor
    {
        /// <summary>
        /// <see cref="WrapperExceptionExtractor"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public WrapperExceptionExtractor()
        {
        }

        /// <summary>
        /// 指定された例外が他の例外をラップしている場合は展開し、ラップされている全ての例外に対して <see cref="Throw(string, Exception, bool) "/> メソッドを実行します。
        /// ラップしていない場合はその例外に対して直接呼び出します。
        /// </summary>
        /// <param name="senderName">例外の発生元の名前。</param>
        /// <param name="exception">解決する例外。</param>
        public void Resolve(string senderName, Exception exception)
        {
            bool isWrapperException = false;
            switch (exception)
            {
                case AggregateException ex:
                    foreach (Exception innerException in ex.InnerExceptions)
                    {
                        Resolve(senderName, innerException);
                    }
                    isWrapperException = true;
                    break;

                case TypeInitializationException ex:
                    Resolve(senderName, ex.InnerException);
                    isWrapperException = true;
                    break;

                case TargetInvocationException ex:
                    Resolve(senderName, ex.InnerException);
                    isWrapperException = true;
                    break;

                case ReflectionTypeLoadException ex:
                    foreach (Exception innerException in ex.LoaderExceptions)
                    {
                        Resolve(senderName, innerException);
                    }
                    isWrapperException = true;
                    break;
            }

            Throw(senderName, exception, isWrapperException);
        }

        /// <summary>
        /// 検知した例外を再度スローします。
        /// </summary>
        /// <remarks>
        /// このメソッドはオーバーライド可能です。
        /// </remarks>
        /// <param name="senderName">例外の発生元の名前。</param>
        /// <param name="exception">スローする例外。</param>
        /// <param name="isWrapperException"><paramref name="exception"/> が他の例外をラップした例外であるかどうか。</param>
        protected virtual void Throw(string senderName, Exception exception, bool isWrapperException)
        {
            ExceptionDispatchInfo.Capture(exception).Throw();
        }
    }
}
