module.exports = async ({ github, context, core, io }) => {
  const owner = context.repo.owner;
  const repo = context.repo.repo;
  const { data: headBranch } = await github.repos.getBranch({
    owner,
    repo,
    branch: context.ref,
  });
  let headBranchName = headBranch.name;
  const branchSections = headBranchName.split("/");
  const indexPadding = branchSections[1].length;

  let baseBranch = null;
  let branchIndex = Number.parseInt(branchSections[1]);

  if (Number.isNaN(branchIndex)) {
    throw new Error(
      `"${headBranchName}" can not be parsed to update branch series. Checkout you "on.push.branch" property to be sure it tracks only branches with names like "foo/01" -> "foo/**"`
    );
  }

  do {
    branchIndex++;
    const formattedIndex = branchIndex.toString().padStart(indexPadding, "0");
    const baseBranchName = `${branchSections[0]}/${formattedIndex}`;

    let baseBranch;

    try {
      const response = await github.repos.getBranch({
        owner,
        repo,
        branch: baseBranchName,
      });

      baseBranch = response.data;
    } catch (err) {
      if (err.status && err.status === 404) {
        console.log(`Branch ${baseBranchName} not found`);
      } else {
        throw err;
      }

      baseBranch = null;
    }

    if (baseBranch) {
      console.log(`Merging ${headBranchName} into ${baseBranchName}`);
      await github.repos.merge({
        owner,
        repo,
        base: baseBranchName,
        head: headBranchName,
      });

      headBranchName = baseBranchName;
    }
  } while (baseBranch);

  return `All branches after "${headBranch.name}" are up to date`;
};
