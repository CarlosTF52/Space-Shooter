using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _bossSpeed;

    [SerializeField]
    private bool _inDownPosition;

    [SerializeField]
    private GameObject[] _turrets;

    [SerializeField]
    private GameObject _bossExplosionPrefab;

    [SerializeField]
    private SmallBossTurret[] _smallTurret;

    [SerializeField]
    private BossTurret _laserTurret;

    [SerializeField]
    private GameObject _shieldsVisual;

    

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 8.7f, 0);
        _shieldsVisual.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > 2f)
        {
            MoveDown();           
        }
        else if(transform.position.y >= 2.0f || transform.position.y <= 2.1f)
        {
            _smallTurret[0].InPosition();
            _smallTurret[1].InPosition();
            _smallTurret[2].InPosition();
            _smallTurret[3].InPosition();
            _laserTurret.InPosition();
            _shieldsVisual.SetActive(false);
        }

        if (_turrets[0].activeSelf && _turrets[1].activeSelf && _turrets[2].activeSelf && _turrets[3].activeSelf && _turrets[4].activeSelf)
        {

            Instantiate(_bossExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);

        }

    }



    private void MoveDown()
    {
        transform.Translate(Vector3.down * _bossSpeed * Time.deltaTime);
        
    }

   
}
