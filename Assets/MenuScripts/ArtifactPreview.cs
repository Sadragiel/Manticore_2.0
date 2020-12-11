using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Artifacts;

public class ArtifactPreview : MonoBehaviour
{
    public Button clickDetector;
    public Image image;
    public Image background;

    public int selfIndex;
    public int holderIndex;
    public void Focus()
    {
        Color c = Color.green;
        c.a = .7f;
        background.color = c;
    }

    public void Blur()
    {
        Color c = Color.white;
        c.a = .7f;
        background.color = c;
    }

    public void setArtifact(Artifact artifact, int selfIndex, int holderIndex, ArtifactManagement manager)
    {
        image.sprite = artifact.image;
        this.selfIndex = selfIndex;
        this.holderIndex = holderIndex;

        clickDetector.onClick.AddListener(() => {
            manager.SetSelectedArtifact(this);
        });
    }
}
