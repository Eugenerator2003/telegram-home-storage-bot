using System;

namespace StorageBot.Application.UseCases.Storing.Containers.UpdateContainerName
{
    public class UpdateContainerNameInput : StorageInput
    {
        public int ContainerId { get; }
        public string ContainerName { get; }
        public UpdateContainerNameInput(int userId, int storageOwnershipId, int containerId, string containerName)
            : base(userId, storageOwnershipId)
        {
            ContainerId = containerId;
            ContainerName = containerName;
        }
    }
}