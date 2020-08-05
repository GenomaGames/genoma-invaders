module.exports = async ({ github, context, core, io }) => {
  const owner = context.repo.owner;
  const repo = context.repo.repo;
  const { data: headBranch } = await github.repos.getBranch({
    owner,
    repo,
    branch: context.ref,
  });
  const head = headBranch.name;
  const branchSections = head.split("/");
  const padding = branchSections[1].length;
  const next = Number.parseInt(branchSections[1]) + 1;
  const nextId = next.toString().padStart(padding, "0");
  const base = `${branchSections[0]}/${nextId}`;

  let baseBranch;

  try {
    const response = await github.repos.getBranch({
      owner,
      repo,
      branch: base,
    });

    baseBranch = response.data;
  } catch (err) {
    if (err.status && err.status === 404) {
      console.log(`Branch ${base} not found`);
    } else {
      throw err;
    }
  }

  if (baseBranch) {
    return github.repos.merge({
      owner,
      repo,
      base,
      head,
    });
  } else {
    return;
  }
};
