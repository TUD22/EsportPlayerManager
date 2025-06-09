using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace EsportPlayerManager.Models;

public partial class Player: ObservableObject
{
    public int PlayerId { get; set; }
    public string Nickname { get; set; }
    private int _skill;
    public int Skill
    {
        get => _skill;
        set
        {
            SetProperty(ref _skill, Math.Clamp(value, 0, 10));
        }
    }

    private int _stress;
    public int Stress
    {
        get => _stress;
        set
        {
            SetProperty(ref _stress, Math.Clamp(value, 0, 10));
        }
    }
    [ObservableProperty] private int money;
}