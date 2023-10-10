// ----------------------------------------------------------------------------
// <copyright file="CustomTypes.cs" company="Exit Games GmbH">
//   PhotonNetwork Framework for Unity - Copyright (C) 2018 Exit Games GmbH
// </copyright>
// <summary>
// Sets up support for Unity-specific types. Can be a blueprint how to register your own Custom Types for sending.
// </summary>
// <author>developer@exitgames.com</author>
// ----------------------------------------------------------------------------


namespace Photon.Pun
{
    using UnityEngine;
    using Photon.Realtime;
    using ExitGames.Client.Photon;
    using System.Collections.Generic;
    using System;

    /// <summary>
    /// Internally used class, containing de/serialization method for PUN specific classes.
    /// </summary>
    internal static class CustomTypes
    {
        /// <summary>Register de/serializer methods for PUN specific types. Makes the type usable in RaiseEvent, RPC and sync updates of PhotonViews.</summary>
        internal static void Register()
        {
            PhotonPeer.RegisterType(typeof(Player), (byte) 'P', SerializePhotonPlayer, DeserializePhotonPlayer);
            PhotonPeer.RegisterType(typeof(List<int>), (byte)'L',SerializeList, DeserializeList);
        }

        private static byte[] SerializeList(object list)
        {
            List<int> intList = (List<int>)list;
            byte[] data = new byte[intList.Count * sizeof(int)];
            Buffer.BlockCopy(intList.ToArray(), 0, data, 0, data.Length);
            return data;
        }

        // Ph??ng th?c Deserialize cho List<int>
        private static object DeserializeList(byte[] data)
        {
            List<int> intList = new List<int>();
            for (int i = 0; i < data.Length; i += sizeof(int))
            {
                int value = BitConverter.ToInt32(data, i);
                intList.Add(value);
            }
            return intList;
        }
        #region Custom De/Serializer Methods

        public static readonly byte[] memPlayer = new byte[4];

        private static short SerializePhotonPlayer(StreamBuffer outStream, object customobject)
        {
            int ID = ((Player) customobject).ActorNumber;

            lock (memPlayer)
            {
                byte[] bytes = memPlayer;
                int off = 0;
                Protocol.Serialize(ID, bytes, ref off);
                outStream.Write(bytes, 0, 4);
                return 4;
            }
        }

        private static object DeserializePhotonPlayer(StreamBuffer inStream, short length)
        {
            if (length != 4)
            {
                return null;
            }

            int ID;
            lock (memPlayer)
            {
                inStream.Read(memPlayer, 0, length);
                int off = 0;
                Protocol.Deserialize(out ID, memPlayer, ref off);
            }

            if (PhotonNetwork.CurrentRoom != null)
            {
                Player player = PhotonNetwork.CurrentRoom.GetPlayer(ID);
                return player;
            }
            return null;
        }

        #endregion
    }
}