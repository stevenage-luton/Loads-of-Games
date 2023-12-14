using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterListController
{
    // UXML template for list entries
    VisualTreeAsset ListEntryTemplate;

    // UI element references
    ListView CharacterList;
    Label CharClassLabel;
    Label CharNameLabel;
    VisualElement CharPortrait;

    public void InitializeCharacterList(VisualElement root, VisualTreeAsset listElementTemplate)
    {
        EnumerateAllCharacters();

        // Store a reference to the template for the list entries
        ListEntryTemplate = listElementTemplate;

        // Store a reference to the character list element
        CharacterList = root.Q<ListView>("character-list");

        // Store references to the selected character info elements
        CharClassLabel = root.Q<Label>("character-class");
        CharNameLabel = root.Q<Label>("character-name");
        CharPortrait = root.Q<VisualElement>("character-portrait");

        FillCharacterList();

        // Register to get a callback when an item is selected
        CharacterList.selectionChanged += OnCharacterSelected;
    }

    List<CharacterData> AllCharacters;

    void EnumerateAllCharacters()
    {
        AllCharacters = new List<CharacterData>();
        AllCharacters.AddRange(Resources.LoadAll<CharacterData>("Characters"));
    }

    void FillCharacterList()
    {
        // Set up a make item function for a list entry
        CharacterList.makeItem = () =>
        {
            // Instantiate the UXML template for the entry
            var newListEntry = ListEntryTemplate.Instantiate();

            // Instantiate a controller for the data
            var newListEntryLogic = new CharacterListEntryController();

            // Assign the controller script to the visual element
            newListEntry.userData = newListEntryLogic;

            // Initialize the controller script
            newListEntryLogic.SetVisualElement(newListEntry);

            // Return the root of the instantiated visual tree
            return newListEntry;
        };

        // Set up bind function for a specific list entry
        CharacterList.bindItem = (item, index) =>
        {
            (item.userData as CharacterListEntryController).SetCharacterData(AllCharacters[index]);
        };

        // Set a fixed item height
        CharacterList.fixedItemHeight = 45;

        // Set the actual item's source list/array
        CharacterList.itemsSource = AllCharacters;
    }

    void OnCharacterSelected(IEnumerable<object> selectedItems)
    {
        // Get the currently selected item directly from the ListView
        var selectedCharacter = CharacterList.selectedItem as CharacterData;

        // Handle none-selection (Escape to deselect everything)
        if (selectedCharacter == null)
        {
            // Clear
            CharClassLabel.text = "";
            CharNameLabel.text = "";
            CharPortrait.style.backgroundImage = null;

            return;
        }

        // Fill in character details
        CharClassLabel.text = selectedCharacter.Class.ToString();
        CharNameLabel.text = selectedCharacter.CharacterName;
        CharPortrait.style.backgroundImage = new StyleBackground(selectedCharacter.PortraitImage);
    }
}