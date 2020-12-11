using Assets.Artifacts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactManagement : MonoBehaviour
{
    public GameObject self;

    List<Artifact> heroEquipped;
    List<Artifact> heroBag;
    List<Artifact> targetStock;

    public Sprite heroIcon;
    public Sprite bagIcon;
    public Sprite locationIcon;

    public GameObject endTurnButton;
    public GameObject openDialogButton;
    public GameObject closeDialogButton;

    public GameObject[] artifactHolders;

    ArtifactHolder[] holders;

    public ArtifactHolder artifactListPrefab;

    int selectedArtifactIndex;
    int selectedHolderIndex;
    ArtifactPreview selectedPreview;
    
    public void SetSelectedArtifact( ArtifactPreview selectedPreview)
    {
        if(this.selectedPreview != null)
        {
            this.selectedPreview.Blur();
            (List<Artifact> list, ArtifactHolder holder) oldContainer = getArtifactContainers(this.selectedPreview.holderIndex);
            
            oldContainer.holder?.UpdateButtonHolderVisibility(false);
        }

        this.selectedArtifactIndex = selectedPreview.selfIndex;
        this.selectedHolderIndex = selectedPreview.holderIndex;
        this.selectedPreview = selectedPreview;
        this.selectedPreview.Focus();

        (List<Artifact> list, ArtifactHolder holder) newContainer = getArtifactContainers(selectedHolderIndex);
        newContainer.holder?.UpdateButtonHolderVisibility(true);
    }

    (List<Artifact>, ArtifactHolder) getArtifactContainers(int index)
    {
        List<Artifact> artifactList = index == 0 ? heroEquipped : index == 1 ? heroBag : targetStock;
        ArtifactHolder previousHolder = holders[index];
        return (artifactList, previousHolder);
    }

    public void MoveSelectedItem(int delta)
    {
        (List<Artifact> list, ArtifactHolder holder) previousContainer = getArtifactContainers(selectedHolderIndex);
        Artifact artifact = previousContainer.list[selectedArtifactIndex];
        previousContainer.list.RemoveAt(selectedArtifactIndex);
        previousContainer.holder.UpdateButtonHolderVisibility(false);

        int targetHolderIndex = (selectedHolderIndex + delta + holders.Length) % holders.Length;

        (List<Artifact> list, ArtifactHolder holder) newContainer = getArtifactContainers(targetHolderIndex);
        newContainer.list.Add(artifact);

        newContainer.holder.SetItemToHolder(selectedPreview);
        selectedPreview.Blur();
        selectedPreview.holderIndex = targetHolderIndex;
        selectedPreview.selfIndex = newContainer.list.Count - 1;
    }

    public void OpenDialog (
        List<Artifact> heroEquipped,
        List<Artifact> heroBag,
        List<Artifact> targetStock,
        bool interactionWithHero
    ) {
        this.heroEquipped = heroEquipped;
        this.heroBag = heroBag;
        this.targetStock = targetStock;

        int numberOfHolders = targetStock == null ? 2 : 3;

        holders = new ArtifactHolder[numberOfHolders];
        for(int i = 0; i < numberOfHolders; i++)
        {
            holders[i] = Instantiate(artifactListPrefab);
            holders[i].transform.SetParent(artifactHolders[i].transform);
        }

        holders[0].SetIcon(heroIcon);
        holders[0].DrawList(heroEquipped, 0, this);
        holders[1].SetIcon(bagIcon);
        holders[1].DrawList(heroBag, 1, this);

        if (targetStock != null)
        {
            holders[2].SetIcon(interactionWithHero ? heroIcon : locationIcon);
            holders[2].DrawList(targetStock, 2, this);
        }

        updateVisibilityOfSidePanelButtons(true);
        self.SetActive(true);
    }

    public void CloseDialog()
    {
        updateVisibilityOfSidePanelButtons(false);
        self.SetActive(false);

        foreach (ArtifactHolder holder in holders)
        {
            holder.transform.parent = null;
        }
    }

    void updateVisibilityOfSidePanelButtons(bool isOpened)
    {
        closeDialogButton.SetActive(isOpened);
        endTurnButton.SetActive(!isOpened);
        openDialogButton.SetActive(!isOpened);
    }
}
