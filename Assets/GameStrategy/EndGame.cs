using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameStrategy
{
    class EndGame : GameStrategy
    {
        GameObject endgameDialog;
        Image endgameDialogImage;
        Text message;
        Sprite victoryImage;
        Sprite loseImage;

        public EndGame(GameObject endgameDialog, Image endgameDialogImage, Text message, Sprite victoryImage, Sprite loseImage)
        {
            this.endgameDialog = endgameDialog;
            this.endgameDialogImage = endgameDialogImage;
            this.message = message;
            this.victoryImage = victoryImage;
            this.loseImage = loseImage;
        }

        public override void Update()
        {
        }

        public override void Start()
        {
            EndGameState state = GameManager.Instance.endGameState;
            message.text = GetMessage(state);
            endgameDialogImage.sprite = GetImageSprite(state);
            endgameDialog.SetActive(true);
        }

        string GetMessage(EndGameState state)
        {
            switch(state)
            {
                case EndGameState.Victory:
                    return "THE VICTORY";
                case EndGameState.GreatVictory:
                    return "THE GREAT VICTORY";
                case EndGameState.Lose:
                    return "THE DEFEAT";
                case EndGameState.GreatLose:
                    return "THE GREAT DEFEAT";
            }
            return "";
        }

        Sprite GetImageSprite(EndGameState state)
        {
            switch (state)
            {
                case EndGameState.Victory:
                case EndGameState.GreatVictory:
                    return victoryImage;
                case EndGameState.Lose:
                case EndGameState.GreatLose:
                    return loseImage;
            }
            return null;
        }
    }
}
