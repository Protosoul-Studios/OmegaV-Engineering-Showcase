#!/usr/bin/env python3
"""Dependency-free repository guard for the public-facing repository tree.

This is a tree/content check, not Git history scanning or publication approval.
"""
from __future__ import annotations

import re
import sys
from pathlib import Path
from urllib.parse import unquote

ROOT = Path(__file__).resolve().parent.parent
errors: list[str] = []
markdown_files = list(ROOT.rglob("*.md"))
markdown_link = re.compile(r"!?\[[^\]]+\]\(([^)]+)\)")
forbidden_paths = {"Assets", "ProjectSettings", "Packages", "Library", "source-samples", "evidence"}
# Internal workflow handoffs belong in the private archive, not the public portfolio.
private_admin_files = {
    "AGENTS.md",
    "PROJECT_STATE.md",
    "docs/PUBLICATION_CHECKLIST.md",
    "docs/demonstration-plan.md",
}
forbidden_placeholders = re.compile(r"\[(?:PRECISE CLAIM|FILE\.cs|APPROVED PATH|SYSTEM|LINK|SHA)\]|\b(?:TODO|TBD|FIXME):")
private_repo_url = re.compile(r"https?://(?:www\.)?github\.com/Protosoul-Studios/OmegaV-AI-Battle-Champions", re.I)
# Approved future gameplay clips and preview stills belong in one reviewable folder.
media_extensions = {".mp4", ".webm", ".png", ".jpg", ".jpeg", ".webp"}
# Compiled/binary game material must never be part of a public showcase.
blocked_extensions = {
    ".prefab", ".unity", ".asset", ".dll", ".exe", ".fbx",
    ".wav", ".mp3", ".apk", ".aab", ".unitypackage", ".pdb", ".blend",
}
sensitive_extensions = {".p12", ".pfx", ".pem", ".key", ".keystore"}


for path in ROOT.rglob("*"):
    if not path.is_file() or ".git" in path.parts:
        continue
    relative = path.relative_to(ROOT)
    if relative.as_posix() in private_admin_files:
        errors.append(f"internal planning file in public showcase: {relative}")
    if any(piece in forbidden_paths for piece in relative.parts):
        errors.append(f"disallowed production/template path: {relative}")
    if path.suffix == ".cs" and relative.parts[:2] != ("samples", "independent-models"):
        errors.append(f"unexpected C# distribution path: {relative}")
    suffix = path.suffix.lower()
    if suffix in blocked_extensions:
        errors.append(f"unreviewed compiled binary or Unity project content: {relative}")
    if suffix in media_extensions and relative.parts[:2] != ("media", "videos"):
        errors.append(f"gameplay media outside media/videos: {relative}")
    if (relative.name == ".env" or relative.name.startswith(".env.")
            or relative.name in {"id_rsa", "id_ed25519"}
            or suffix in sensitive_extensions):
        errors.append(f"sensitive configuration or private-key file: {relative}")

for path in markdown_files:
    relative = path.relative_to(ROOT)
    content = path.read_text(encoding="utf-8")
    if forbidden_placeholders.search(content):
        errors.append(f"unresolved template/maintenance placeholder: {relative}")
    if private_repo_url.search(content):
        errors.append(f"private production URL exposed: {relative}")
    for match in markdown_link.finditer(content):
        target = match.group(1).split(" ", 1)[0].strip("<>")
        if not target or target.startswith(("#", "http:", "https:", "mailto:")):
            continue
        resolved = (path.parent / unquote(target.split("#", 1)[0].split("?", 1)[0])).resolve()
        if not resolved.is_relative_to(ROOT) or not resolved.exists():
            errors.append(f"broken or escaping link: {relative} -> {target}")

if errors:
    print("Showcase tree audit FAILED:")
    for problem in errors:
        print(" - " + problem)
    sys.exit(1)

print(f"SHOWCASE TREE AUDIT PASSED: {len(markdown_files)} Markdown files, links and distribution paths checked")
print("NOTE: Media still requires manual rights/privacy approval. This does not audit unreachable commits, all Git refs, copyright clearance or gameplay.")
