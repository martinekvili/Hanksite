﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common.Game
{
    [ServiceContract(CallbackContract = typeof(IGameServiceCallback))]
    public interface IGameService
    {
        [OperationContract(IsOneWay = true)]
        void ChooseColour(int colour);

        [OperationContract(IsOneWay = true)]
        void DisconnectFromGame();

        [OperationContract(IsOneWay = false)]
        GameSnapshot ReconnectToGame(int gameId);
    }

    public interface IGameServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void SendGameSnapshot(GameSnapshot snapshot);

        [OperationContract(IsOneWay = true)]
        void DoNextStep(GameSnapshotForNextPlayer snapshot);

        [OperationContract(IsOneWay = true)]
        void SendTimedOut();

        [OperationContract(IsOneWay = true)]
        void SendGameOver(GameSnapshot snapshot);
    }
}
