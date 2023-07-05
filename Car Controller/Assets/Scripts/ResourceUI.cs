using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceUI : MonoBehaviour {

    private void Awake() {
        Transform resourceTemplate = transform.Find("resourceTemplate");
        resourceTemplate.gameObject.SetActive(false);
    }

    private void Start() {
        Transform resourceTemplate = transform.Find("resourceTemplate");

        List<ResourceTypeSO> resourceTypeSOList = new List<ResourceTypeSO> {
            GameAssets.Instance.rubberResourceType,
            GameAssets.Instance.steelResourceType,
            GameAssets.Instance.aluminiumResourceType,
        };

        float resourceTransformWidth = 160f;
        float index = 0;

        foreach (ResourceTypeSO resourceTypeSO in resourceTypeSOList) {
            Transform resourceTransform = Instantiate(resourceTemplate, transform);
            resourceTransform.gameObject.SetActive(true);

            RectTransform resourceRectTransform = resourceTransform.GetComponent<RectTransform>();
            resourceRectTransform.anchoredPosition = new Vector2(resourceTransformWidth * index, 0);

            ResourceSingle resourceSingle = new ResourceSingle(resourceTypeSO, resourceTransform);

            index++;
        }
    }

    private class ResourceSingle {

        private ResourceTypeSO resourceTypeSO;
        private Transform transform;
        private TextMeshProUGUI text;

        public ResourceSingle(ResourceTypeSO resourceTypeSO, Transform transform) {
            this.resourceTypeSO = resourceTypeSO;
            this.transform = transform;
            transform.Find("image").GetComponent<Image>().sprite = resourceTypeSO.sprite;
            text = transform.Find("text").GetComponent<TextMeshProUGUI>();

            ResourceManager.Instance.OnResourceAmountChanged += Instance_OnResourceAmountChanged;

            UpdateText();
        }

        private void Instance_OnResourceAmountChanged(object sender, System.EventArgs e) {
            UpdateText();
        }

        private void UpdateText() {
            text.text = ResourceManager.Instance.GetResourceAmount(resourceTypeSO).ToString();
        }

    }

}
