//===================================================================================
// Microsoft Developer & Platform Evangelism
//=================================================================================== 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// Copyright (c) Microsoft Corporation.  All Rights Reserved.
// This code is released under the terms of the MS-LPL license, 
// http://microsoftnlayerapp.codeplex.com/license
//===================================================================================

#region

using System;
using System.Diagnostics;
using System.Globalization;
using System.Security;
using System.Text;
using Newtonsoft.Json;

#endregion

namespace LogMVC.CrossCutting.Logging
{
    /// <summary>
    ///   Implementation of contract <see cref="ILogger" />
    ///   using System.Diagnostics API.
    /// </summary>
    public sealed class TraceSourceLog : ILogger
    {
        #region Fields

        private readonly TraceSource _source;

        #endregion

        #region  Constructor

        /// <summary>
        ///   Create a new instance of this trace manager
        /// </summary>
        public TraceSourceLog()
        {
            // Create default source
            _source = new TraceSource("TRACER");
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///   Trace internal message in configured listeners
        /// </summary>
        /// <param name="eventType"> Event type to trace </param>
        /// <param name="message"> Message of event </param>
        private void TraceInternal(TraceEventType eventType, string message)
        {
            if (_source != null)
            {
                try
                {
                    _source.TraceEvent(eventType, (int)eventType, message);
                }
                catch (SecurityException exSec)
                {
                    //Cannot access to file listener or cannot have
                    //privileges to write in event log etc...
                    throw new Exception("Erro de permissão para gravar LOG.", exSec.InnerException);
                }
                catch
                {
                    //não faz nada para não impactar o sistema 
                }
            }
        }


        private string Serializacao(params object[] list)
        {
            if (list == null) return string.Empty;
            {
                if (list.GetLength(0) > 0)
                {
                    var sb = new StringBuilder();

                    for (int i = 0; i < list.Length; i++)
                    {
                        try
                        {
                            sb.Append("<InicioLogArg>");
                            sb.Append(
                                JsonConvert.SerializeObject(list[i],
                                    new JsonSerializerSettings()
                                    {
                                        DefaultValueHandling = DefaultValueHandling.Ignore,
                                        PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                                        Formatting = Formatting.Indented
                                    }));

                            sb.Append("<FimLogArg>");
                        }
                        catch (JsonSerializationException exJson)
                        {
                            TraceInternal(TraceEventType.Warning, "Erro na Serializacao do Log:" + exJson.Message + list[i]);
                        }
                        catch (Exception ex)
                        {
                            TraceInternal(TraceEventType.Warning, "Erro na Serializacao do Log Exception:" + ex.Message);
                        }
                    }
                    return sb.ToString();
                }

            }
            return string.Empty;

        }

        #endregion

        #region ILogger Members

        /// <summary>
        ///   <see cref="ILogger" />
        /// </summary>
        /// <param name="message"> <see cref="ILogger" /> </param>
        /// <param name="args"> <see cref="ILogger" /> </param>
        public void Info(string message, params object[] args)
        {
            if (_source.Switch.Level >= SourceLevels.Information)
            {
                try
                {
                    if (!String.IsNullOrWhiteSpace(message))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(message).Append(Serializacao(args));

                        TraceInternal(TraceEventType.Information, sb.ToString());
                    }
                }
                catch (Exception ex)
                {
                    TraceInternal(TraceEventType.Error, ex.Message);
                }
            }
        }

        /// <summary>
        ///   <see cref="ILogger" />
        /// </summary>
        /// <param name="message"> <see cref="ILogger" /> </param>
        /// <param name="args"> <see cref="ILogger" /> </param>
        public void Warning(string message, params object[] args)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(message))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(message).Append(Serializacao(args));

                    TraceInternal(TraceEventType.Warning, sb.ToString());
                }
            }
            catch (Exception ex)
            {
                TraceInternal(TraceEventType.Error, ex.Message);
            }
        }

        /// <summary>
        ///   <see cref="ILogger" />
        /// </summary>
        /// <param name="message"> <see cref="ILogger" /> </param>
        /// <param name="args"> <see cref="ILogger" /> </param>
        public void Error(string message, params object[] args)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(message))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(message).Append(Serializacao(args));

                    TraceInternal(TraceEventType.Error, sb.ToString());
                }
            }
            catch (Exception ex)
            {
                TraceInternal(TraceEventType.Error, ex.Message);
            }
        }

        /// <summary>
        ///   <see cref="ILogger" />
        /// </summary>
        /// <param name="message"> <see cref="ILogger" /> </param>
        /// <param name="exception"> <see cref="ILogger" /> </param>
        /// <param name="args"> <see cref="ILogger" /> </param>
        public void Error(string message, Exception exception, params object[] args)
        {
            if (!String.IsNullOrWhiteSpace(message) && exception != null)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(message).Append(Serializacao(args));
                    var messageToTrace = sb.ToString();

                    var exceptionData = exception.ToString();
                    // The ToString() create a string representation of the current exception

                    TraceInternal(TraceEventType.Error,
                                  string.Format(CultureInfo.InvariantCulture, "{0}\nMensagem de Exceção:{1}", 
                                  messageToTrace,
                                  exceptionData));
                }
                catch (Exception ex)
                {
                    TraceInternal(TraceEventType.Error, ex.Message);
                }
            }
        }

        /// <summary>
        ///   <see cref="ILogger" />
        /// </summary>
        /// <param name="message"> <see cref="ILogger" /> </param>
        /// <param name="args"> <see cref="ILogger" /> </param>
        public void Debug(string message, params object[] args)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(message))
                {
                    //var messageToTrace = string.Format(CultureInfo.InvariantCulture, string.Concat(message, Serializacao(args)));
                    StringBuilder sb = new StringBuilder();
                    sb.Append(message).Append(Serializacao(args));

                    TraceInternal(TraceEventType.Verbose, sb.ToString());
                }
            }
            catch (Exception ex)
            {
                TraceInternal(TraceEventType.Error, ex.Message);
            }
        }

        /// <summary>
        ///   <see cref="ILogger" />
        /// </summary>
        /// <param name="message"> <see cref="ILogger" /> </param>
        /// <param name="exception"> <see cref="ILogger" /> </param>
        /// <param name="args"> <see cref="ILogger" /> </param>
        public void Debug(string message, Exception exception, params object[] args)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(message) && exception != null)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(message).Append(Serializacao(args));

                    var messageToTrace = sb.ToString();

                    var exceptionData = exception.ToString();
                    // The ToString() create a string representation of the current exception

                    TraceInternal(TraceEventType.Error, string.Format(CultureInfo.InvariantCulture, "{0} Exception:{1}", messageToTrace, exceptionData));
                }
            }
            catch (Exception ex)
            {
                TraceInternal(TraceEventType.Error, ex.Message);
            }
        }

        /// <summary>
        ///   <see cref="ILogger" />
        /// </summary>
        /// <param name="item"> <see cref="ILogger" /> </param>
        public void Debug(object item)
        {
            if (item != null)
            {
                TraceInternal(TraceEventType.Verbose, item.ToString());
            }
        }

        /// <summary>
        ///   <see cref="ILogger" />
        /// </summary>
        /// <param name="message"> <see cref="ILogger" /> </param>
        /// <param name="args"> <see cref="ILogger" /> </param>
        public void Fatal(string message, params object[] args)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(message))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(message).Append(Serializacao(args));

                    TraceInternal(TraceEventType.Critical, sb.ToString());
                }
            }
            catch (Exception ex)
            {
                TraceInternal(TraceEventType.Error, ex.Message);
            }
        }

        /// <summary>
        ///   <see cref="ILogger" />
        /// </summary>
        /// <param name="message"> <see cref="ILogger" /> </param>
        /// <param name="exception"> <see cref="ILogger" /> </param>
        public void Fatal(string message, Exception exception, params object[] args)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(message) && exception != null)
                {
                    StringBuilder messageToTrace = new StringBuilder();
                    messageToTrace.Append(message).Append(Serializacao(args));

                    var exceptionData = exception.ToString();
                    // The ToString() create a string representation of the current exception

                    TraceInternal(TraceEventType.Critical, string.Format(CultureInfo.InvariantCulture, "{0} Exception:{1}", messageToTrace, exceptionData));
                }
            }
            catch (Exception ex)
            {
                TraceInternal(TraceEventType.Error, ex.Message);
            }
        }

        #endregion
    }
}