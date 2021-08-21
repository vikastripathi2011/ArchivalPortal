//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : DataLayerBase.cs
// Program Description  : Reusable fucntionalty for DataLayer logic
// Programmed By        : Naushad Ali
// Programmed On        : 10-December-2012 
// Version              : 1.0.0
//==========================================================================================

#region Namespaces

using System;
using System.ServiceModel;

#endregion

namespace WLCaxtonMvcWebPortal.Services
{
    #region ChannelFactoryBase Service Class
    
    public abstract class ChannelFactoryBase<TChannel>
    {
        /// <summary>
        /// This is channel factory base class
        /// </summary>
        #region PageProtectedProperties
        
        protected TChannel Channel { get; private set; }

        #endregion

        #region PagePrivateProperties

        private ChannelFactory<TChannel> Factory { get; set; }

        #endregion

        #region PublicMethods
        
        /// <summary>
        /// This method call close method to close the channel
        /// </summary>
        public virtual void Close()
        {
            if (Factory != null)
            {
                Close(true);
            }
        }

        /// <summary>
        /// This method abort the channel
        /// </summary>
        public virtual void Abort()
        {
            if (Factory != null)
            {
                Factory.Abort();
            }
        }

        #endregion

        #region PrivateMethods
        
        /// <summary>
        /// If any channel is opened then this method close the channel
        /// </summary>
        /// <param name="disposing"></param>
        private void Close(bool disposing)
        {
            if (disposing)
            {
                if (Factory != null)
                {
                    try
                    {
                        if (Factory.State == CommunicationState.Faulted)
                        {
                            Factory.Abort();
                        }
                        else
                        {
                            Factory.Close();
                        }
                    }
                    catch
                    {
                        Factory.Abort();
                    }
                    finally
                    {
                        GC.SuppressFinalize(this);
                    }
                }
            }
        }

        #endregion

        #region ProtectedMethods

        /// <summary>
        /// This is a abstract method.
        /// </summary>
        protected ChannelFactoryBase() { }

        /// <summary>
        /// This method used for open the new channel
        /// </summary>
        /// <param name="endPointConfigurationName"></param>
        protected ChannelFactoryBase(string endPointConfigurationName)
        {
            this.Factory = new ChannelFactory<TChannel>(endPointConfigurationName);
            Factory.Open();
            this.Channel = this.Factory.CreateChannel();
        }

        #endregion
    }

    #endregion
}
