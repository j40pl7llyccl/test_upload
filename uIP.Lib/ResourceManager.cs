using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace uIP.Lib
{
    public static class ResourceManager
    {
        public const string LogDelegate = "LogDelegate";
        public const string LibAgent = "LibAgent";
        public const string PluginServiceName = "PluginService";
        public const string ScriptService = "ScriptService";
        public const string ScriptRunnerFactory = "ScriptRunnerFactory";
        public const string ActionService = "ActionService";
        public const string ScriptEditor = "ScriptEditor";
        public const string SystemUp = "SystemUp";

        private static object _sync = new object();
        private static Dictionary<string, object> _Resources = new Dictionary<string, object>();

        public static void Reg( string name, object res )
        {
            if ( String.IsNullOrEmpty( name ) )
                return;

            Monitor.Enter( _sync );
            if ( _Resources.ContainsKey( name ) )
                _Resources[ name ] = res;
            else
                _Resources.Add( name, res );
            Monitor.Exit( _sync );
        }
        public static void Unreg( string name )
        {
            Monitor.Enter( _sync );
            if ( _Resources.ContainsKey( name ) )
                _Resources.Remove( name );
            Monitor.Exit( _sync );
        }
        public static void Clear()
        {
            Monitor.Enter( _sync );
            _Resources.Clear();
            Monitor.Exit( _sync );
        }
        public static object Get( string name )
        {
            object repo = null;

            Monitor.Enter( _sync );
            if ( _Resources.ContainsKey( name ) ) { 
                repo = _Resources[name];
            }
            Monitor.Exit( _sync );

            return repo;
        }
        public static bool Get<T>( string name, T defaultV, out T result )
        {
            var status = false;
            result = defaultV;

            Monitor.Enter( _sync );
            if ( _Resources.TryGetValue( name, out var item ) && item != null && ( item is T v ) )
            {
                result = v;
                status = true;
            }
            Monitor.Exit( _sync );

            return status;
        }

        public static bool IsSystemUp()
        {
            bool repo = false;
            Monitor.Enter( _sync );
            if ( _Resources.ContainsKey( SystemUp ) )
                repo = ( bool ) _Resources[ SystemUp ];
            Monitor.Exit( _sync );
            return repo;
        }

        public static void InvokeMainThreadFreeResource( object ctx )
        {
            if ( Get( ScriptEditor ) is Form frm && frm != null && frm.InvokeRequired )
            {
                frm.Invoke( new System.Action( () => UDataCarrier.Free( ctx ) ) );
            }
            else
                UDataCarrier.Free( ctx );
        }

        public static bool SystemAvaliable => ULibAgent.Singleton != null && ULibAgent.Singleton.Available;

        /// <summary>
        /// UDataCarrier data from different PC or program using last match can be enable
        /// Remark:
        /// Disable => Some conditions will case type exception such Point in System.Drawing; ReadXml use exactly assembly can avoid this issue
        /// </summary>
        public static bool DataCarrierTypePerfectMatch { get; set; } = true;
    }
}
