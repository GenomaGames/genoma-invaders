module.exports = async ({ github, context, core, io }) => {
  const owner = context.repo.owner;
  const repo = context.repo.repo;
  const { data: headBranch } = await await github.repos.getBranch({
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
  const { data: baseBranch } = await await github.repos.getBranch({
    owner,
    repo,
    branch: base,
  });

  return github.repos.merge({
    owner,
    repo,
    base,
    head,
  });
};
