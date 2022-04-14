using System;

interface INotEquippedWeapon
{
    public string Name { get; }
    public int Damage { get; }
    public int BulletsToOneShot { get; }
    public int FullClipBulletCount { get; }
}

class Weapon : INotEquippedWeapon
{
    public Weapon(string name, int damage, int BulletsToOneShot, int FullClipBulletCount)
    {
        if (damage < 0 || FullClipBulletCount < 0)
            throw new ArgumentOutOfRangeException();

        Name = name;
        Damage = damage;
        this.FullClipBulletCount = FullClipBulletCount;
        this.BulletsToOneShot = BulletsToOneShot;
        BulletsOnClipCount = FullClipBulletCount;
    }

    public Weapon(INotEquippedWeapon weapon)
    {
        Name = weapon.Name;
        Damage = weapon.Damage;
        BulletsToOneShot = weapon.BulletsToOneShot;
        FullClipBulletCount = weapon.FullClipBulletCount;
    }

    public int Damage { get; private set; }
    public string Name { get; private set; }
    public int BulletsToOneShot { get; }
    public int BulletsOnClipCount { get; private set; }
    public int FullClipBulletCount { get; private set; }

    public bool TryShoot()
    {
        return BulletsOnClipCount < BulletsToOneShot;
    }

    public int Shoot()
    {
        BulletsOnClipCount -= BulletsToOneShot;
        return Damage;
    }
}

class Player
{
    private bool _canShooted;

    public Player(int health)
    {
        if (health < 0)
            throw new ArgumentOutOfRangeException();

        Health = health;
        _canShooted = true;
    }

    public int Health { get; private set; }

    public bool TryTakeDamage()
    {
        return _canShooted;
    }

    public void TakeDamage(int damage)
    {
        if (damage < 0)
            throw new ArgumentOutOfRangeException();

        Health -= damage;

        if (Health < 0)
            Death();
    }

    private void Death()
    {
        _canShooted = false;
    }
}

class Bot
{
    private Weapon _weaponInHands;

    public Bot(INotEquippedWeapon weapon)
    {
        if (weapon == null)
            throw new NullReferenceException();

        _weaponInHands = new Weapon(weapon);
    }

    public void OnSeePlayer(Player player)
    {
        if (player == null)
            throw new NullReferenceException();

        if (player.TryTakeDamage())
        {
            if (_weaponInHands.TryShoot())
            {
                int damage = _weaponInHands.Shoot();
                player.TakeDamage(damage);
            }
        }

    }
}