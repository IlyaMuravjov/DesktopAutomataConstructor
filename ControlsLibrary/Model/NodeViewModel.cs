﻿using ControlsLibrary.ViewModel;
using GraphX.Common.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ControlsLibrary.Model
{
    public class NodeViewModel : VertexBase, INotifyPropertyChanged
    {
        public NodeViewModel()
        {
            Attributes = new List<AttributeViewModel>();
            Attributes.Add(new AttributeViewModel("name", TypeEnum.String));
        }

        private string name;
        private bool isInitial;
        private bool isFinal;
        private IList<AttributeViewModel> attributes;

        /// <summary>
        /// Name of the state
        /// </summary>
        public string Name 
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Is state initial
        /// </summary>
        public bool IsInitial 
        { 
            get => isInitial; 
            set
            {
                isInitial = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Is state final
        /// </summary>
        public bool IsFinal 
        { 
            get => isFinal; 
            set
            {
                isFinal = value;
                OnPropertyChanged();
            }
        }

        public IList<AttributeViewModel> Attributes
        {
            get => attributes;
            set
            {
                attributes = value;
                OnPropertyChanged();
            }
        }

        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Overriding of the base method
        /// </summary>
        public override string ToString() => Name;
    }
}
