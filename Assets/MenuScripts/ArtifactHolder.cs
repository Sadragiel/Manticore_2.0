using Assets.Artifacts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ArtifactHolder : MonoBehaviour
{
    public Image icon;
    public GameObject holder;
    public GameObject buttonHolder;

    public ArtifactPreview previewPrefab;

    ArtifactManagement manager;

    public void SetIcon(Sprite iconSprite)
    {
        icon.sprite = iconSprite;
    }

    public void SetItemToHolder(ArtifactPreview preview)
    {
        preview.transform.parent = null;
        preview.transform.SetParent(holder.transform);
    }

    public void DrawList(List<Artifact> artifactList, int selfIndex, ArtifactManagement manager)
    {
        this.manager = manager;
        for (int i = 0; i < artifactList.Count; i++)
        {
            ArtifactPreview preview = Instantiate(previewPrefab, holder.transform);
            preview.setArtifact(artifactList[i], i, selfIndex, manager);
        }
    }

    public void UpdateButtonHolderVisibility(bool state)
    {
        buttonHolder.SetActive(state);
    }

    public void MoveSelectedItem(int delta)
    {
        manager.MoveSelectedItem(delta);
    }
}
