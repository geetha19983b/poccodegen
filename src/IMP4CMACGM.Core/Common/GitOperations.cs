using LibGit2Sharp;
using System;
using System.IO;

namespace IMP4CMACGM.Core.Common
{
    public class GitOperations
    {
        private readonly string _repoSource;
        private readonly string _commitMessage;
        private readonly UsernamePasswordCredentials _credentials;
        private readonly DirectoryInfo _localFolder;
        public GitOperations(string username, string password, string gitRepoUrl, string outputFolderPath)
        {
            var folder = new DirectoryInfo(outputFolderPath);
            _commitMessage = "Creation of API project from Swagger, by API Code Generator!";

            _localFolder = folder;

            _credentials = new UsernamePasswordCredentials
            {
                Username = username,
                Password = password
            };

            _repoSource = gitRepoUrl;

        }
        public void PushCommits(string remoteName, string branchName)
        {
            using (var repo = new Repository(_localFolder.FullName))
            {
                LibGit2Sharp.Commands.Checkout(repo, repo.Branches[branchName]);
                var status = repo.RetrieveStatus();
                #region Git - Stage & Commit Files
                var untrackfiles = status.Untracked;
                
                foreach (var item in untrackfiles)
                {
                    LibGit2Sharp.Commands.Stage(repo, item.FilePath);
                }
                Signature author = new Signature(_credentials.Username.ToString(), _credentials.Username.ToString(), DateTimeOffset.Now);
                repo.Commit(_commitMessage, author, author);
                #endregion

                #region Git - Push only needed files
                var options = new PushOptions
                {
                    CredentialsProvider = (url, usernameFromUrl, types) => _credentials
                };

                repo.Network.Push(repo.Branches[branchName], options);
                #endregion
            }
        }

        public void CreateBranch(string remoteName, string branchName)
        {
            using (var repo = new Repository(_localFolder.FullName))
            {
                repo.CreateBranch(remoteName);
                var localBranch = repo.Branches[branchName];
                var options = new PushOptions
                {
                    CredentialsProvider = (url, usernameFromUrl, types) => _credentials
                };
                repo.Branches.Update(localBranch, b => b.Remote = repo.Network.Remotes["origin"].Name, b => b.UpstreamBranch = localBranch.CanonicalName);
                repo.Network.Push(localBranch, options);

            }
        }

        public void CloneRepo()
        {

            var clonopts = new CloneOptions
            {
                CredentialsProvider = (url, usernameFromUrl, types) => _credentials
            };
            Repository.Clone(_repoSource, _localFolder.FullName, clonopts);

        }

        public bool ListGitBranches(string branchName)
        {
            Branch branches;

            using (var repo = new Repository(_localFolder.FullName))
            {
                branches = repo.Branches[branchName];

            }
            if (branches != null)
                return true;
            else return false;

        }
    }
}
