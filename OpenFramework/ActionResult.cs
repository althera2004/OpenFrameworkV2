namespace OpenFramework
{
    using System;
    using System.Globalization;
    using System.Text;

    /// <summary>Implements a class that represents the result of an operation</summary>
    public sealed class ActionResult
    {
        /// <summary>Initializes a new instance of the ActionResult class.</summary>
        public ActionResult()
        {
            this.Success = false;
            this.MessageError = "No action";
        }

        /// <summary>Gets a default action result for no action occurs</summary>
        public static ActionResult NoAction
        {
            get
            {
                return new ActionResult() { Success = false, MessageError = "No action" };
            }
        }

        /// <summary>Gets a default action result for no action occurs with success result</summary>
        public static ActionResult SuccessNoAction
        {
            get
            {
                return new ActionResult() { Success = true, MessageError = "No action" };
            }
        }

        /// <summary>Gets or sets a value indicating whether if the action has is success or fail</summary>
        public bool Success { get; set; }

        /// <summary>Gets or sets a value indicating whether the message of result</summary>
        public string MessageError { get; set; }

        /// <summary>Gets or sets a value indicating the result value of action</summary>
        public object ReturnValue { get; set; }

        /// <summary>Gets a JSON structure of object</summary>
        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Success"":{0},""MessageError"":""{1}"",""ReturnValue"":""{2}""}}",
                    this.Success ? ConstantValue.True : ConstantValue.False,
                    ToolsJson.JsonCompliant(this.MessageError),
                    ToolsJson.JsonCompliant(this.ReturnValue as string));
            }
        }

        /// <summary>Sets the success of action to true</summary>
        public void SetSuccess()
        {
            this.SetSuccess(string.Empty);
        }

        /// <summary>Sets the success of action to true</summary>
        /// <param name="returnValue">Value to return</param>
        public void SetSuccess(object returnValue)
        {
            this.SetSuccess(string.Empty);
            this.ReturnValue = returnValue;
        }

        /// <summary>Sets the success of action to true</summary>
        /// <param name="returnValue">Value to return</param>
        public void SetSuccess(StringBuilder returnValue)
        {
            if (returnValue == null)
            {
                this.SetSuccess(string.Empty);
                this.ReturnValue = string.Empty;
            }
            else
            {
                this.SetSuccess(string.Empty);
                this.ReturnValue = returnValue.ToString();
            }
        }

        /// <summary>Sets the success of action to true with a message</summary>
        /// <param name="message">Text of message</param>
        public void SetSuccess(string message)
        {
            this.Success = true;
            if (!string.IsNullOrEmpty(message))
            {
                this.ReturnValue = message;
            }
            else
            {
                this.ReturnValue = string.Empty;
            }
        }

        /// <summary>Sets the success of action to true with a message</summary>
        /// <param name="newItemId">Identifier of new item added in database</param>
        public void SetSuccess(int newItemId)
        {
            this.Success = true;
            this.ReturnValue = newItemId.ToString(CultureInfo.CurrentCulture);
        }

        /// <summary>Sets the success of action to true with a message</summary>
        /// <param name="newItemId">Identifier of new item added in database</param>
        public void SetSuccess(long newItemId)
        {
            this.Success = true;
            this.ReturnValue = newItemId.ToString(CultureInfo.CurrentCulture);
        }

        /// <summary>Sets the success of action to false with a message</summary>
        /// <param name="message">Text of message</param>
        public void SetFail(string message)
        {
            this.Success = false;
            if (!string.IsNullOrEmpty(message))
            {
                this.MessageError = message;
            }
            else
            {
                this.MessageError = string.Empty;
            }
        }

        /// <summary>Sets the success of action to false with a message</summary>
        /// <param name="ex">Exception that causes fail</param>
        public void SetFail(Exception ex)
        {
            this.Success = false;
            if (ex != null && !string.IsNullOrEmpty(ex.Message))
            {
                this.MessageError = ex.Message;
            }
            else
            {
                this.MessageError = string.Empty;
            }
        }
    }
}