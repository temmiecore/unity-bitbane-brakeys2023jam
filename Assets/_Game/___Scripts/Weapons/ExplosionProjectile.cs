using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionProjectile : MonoBehaviour
{
    [HideInInspector] public float damage;
    [HideInInspector] public float knockbackStrength;

    public GameObject explosionPrefab;

    public void SetupProjectile(IWeapon weapon)
    {
        damage = weapon.damage;
        knockbackStrength = weapon.knockbackStrength;

        Destroy(gameObject, 20);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<Enemy>().RecieveDamage(damage + GameManager.Instance.playerParameters.additionalDamage);
            collision.GetComponent<Enemy>().Knockback(knockbackStrength);

            Explosion();
            Destroy(gameObject);
        }

        if (collision.tag == "Destructable")
        { /*Destroy destructable*/ }

        if (collision.tag == "TerrainCol")
            Destroy(gameObject);
    }

    private void Explosion()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, 0.5f);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.32f, LayerMask.GetMask("Enemy"));

        foreach (Collider2D collider in colliders)
        {
            collider.GetComponent<Enemy>().RecieveDamage(damage + GameManager.Instance.playerParameters.additionalDamage);
            collider.GetComponent<Enemy>().Knockback(knockbackStrength + 2f);
        }
    }
}
