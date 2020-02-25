using System;
using System.Collections.Generic;
using System.Text;
using LibGit2Sharp;
using System.Linq;
using System.IO;

namespace IMP4CMACGM.Cli
{
    public class InfyGit
    {
        private readonly string _repoSource;
        private readonly UsernamePasswordCredentials _credentials;
        private readonly DirectoryInfo _localFolder;
        public InfyGit(string username,string password,string gitRepoUrl)
        {
            var folder = new DirectoryInfo(@"D:\Work\Projects\eCommerce\Source\Repos\pocgit");
            
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
                LibGit2Sharp.Commands.Stage(repo, @"D:\Work\Projects\eCommerce\Source\Repos\pocgit\test.txt");                
                Signature author = new Signature("Manoj Paliwal", "manoj_paliwal@infosys.com", DateTimeOffset.Now);
                repo.Commit($"Here's a commit  i made!", author, author);
                var options = new PushOptions
                {
                    CredentialsProvider = (url, usernameFromUrl, types) => _credentials
                };
                
                repo.Network.Push(repo.Branches["master"], options);
            }
        }

        public void CreateBranch(string remoteName, string branchName)
        {
            using (var repo = new Repository(_localFolder.FullName))
            {
                repo.CreateBranch("lakxman");
                var localBranch = repo.Branches["lakxman"];
                var options = new PushOptions
                {
                    CredentialsProvider = (url, usernameFromUrl, types) => _credentials
                };
                repo.Branches.Update(localBranch, b => b.Remote = repo.Network.Remotes["origin"].Name,b=>b.UpstreamBranch= localBranch.CanonicalName);
                repo.Network.Push(localBranch, options);
            }
        }

        public void CloneRepo()
        {

            var clonopts = new CloneOptions
            {
                CredentialsProvider = (url, usernameFromUrl, types) => _credentials
            };
            var test = Repository.Clone(_repoSource, _localFolder.FullName, clonopts);

    }
}
}
