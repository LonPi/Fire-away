using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {

    public float maxHitPoints;
    public GameObject[] Slimes;
    public float hitPoints { get; private set; }
    public Transform _transform { get { return transform; } private set { } }
    SpriteRenderer _spriteRenderer;
    bool _isDead;
    bool _isTakingDamage;
    bool _lowHP;

    public AudioClip treeLowSFX;
    public AudioClip treeDeadSFX;

    void Start () {
        hitPoints = maxHitPoints;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _isDead = false;
        _lowHP = false;
        
    }
	
	void Update () {

        if (hitPoints <= 10 && !_lowHP)
        {
            _lowHP = true;
            SoundManager.instance.TreePlaySingle(SoundManager.instance.treeLowSFX);
        }

        if (hitPoints <= 0 && !_isDead)
        {
            GameManager.instance.GameOver();
            _isDead = true;
            SoundManager.instance.TreePlaySingle(SoundManager.instance.treeDeadSFX);
        }
    }

    public void TakeDamage(float damage)
    {
        if (hitPoints <= 0) hitPoints = 0;
        else hitPoints -= damage;
        StartCoroutine(_IndicateBeingDamaged());
        float percentHitpointsLeft = hitPoints / maxHitPoints;

        if (percentHitpointsLeft >= 0.3 && percentHitpointsLeft < 0.4)
        {
            Slimes[0].SetActive(true);
        }
        if (percentHitpointsLeft >= 0.2 && percentHitpointsLeft < 0.3)
        {
            Slimes[0].SetActive(false);
            Slimes[1].SetActive(true);
        }
        if (percentHitpointsLeft >= 0.1 && percentHitpointsLeft < 0.2)
        {
            Slimes[0].SetActive(false);
            Slimes[1].SetActive(false);
            Slimes[2].SetActive(true);
        }
        if (percentHitpointsLeft < 0.1)
        {
            Slimes[0].SetActive(false);
            Slimes[1].SetActive(false);
            Slimes[2].SetActive(false);
            Slimes[3].SetActive(true);

        }
    }

    IEnumerator _IndicateBeingDamaged()
    {
        _spriteRenderer.color = new Color32(0xFF, 0x78, 0x78, 0xFF);
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
    }
}
