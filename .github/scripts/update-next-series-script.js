module.exports = async ({ github, context, core, io }) => {
  const owner = context.repo.owner;
  const repo = context.repo.repo;
  const head = context.ref;
  const branchSections = head.split("/");
  console.log(branchSections);
  const padding = branchSections[1].length;
  const next = Number.parseInt(branchSections[1]) + 1;
  const nextId = next.toString().padStart(padding, "0");
  const base = `${branchSections[0]}/${nextId}`;
  const { data: branch } = await await github.repos.getBranch({
    owner,
    repo,
    base,
  });

  if (branch) {
    return github.repos.merge({
      owner,
      repo,
      base,
      head,
    });
  } else {
    return null;
  }
};
