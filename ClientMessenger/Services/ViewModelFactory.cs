﻿using ClientMessenger.Enums;
using ClientMessenger.ViewModels;

namespace ClientMessenger.Services;

public class ViewModelFactory
{
    public ViewModel CreateViewModel(ViewModelEnum type)
    {
        return type switch
        {
            ViewModelEnum.ClientChatViewModel => App.Container.GetInstance<ClientChatViewModel>(),
            ViewModelEnum.TemplateSecondViewModel => App.Container.GetInstance<TemplateSecondViewModel>()
        };
    }
}