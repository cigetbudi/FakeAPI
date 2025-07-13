#!/bin/bash

read -p "Masukkan nama lama project (case-sensitive): " OLD_NAME
read -p "Masukkan nama baru project: " NEW_NAME

# Step 1: Ganti nama folder
find . -depth -type d -name "*$OLD_NAME*" | while read dir; do
    newdir=$(echo "$dir" | sed "s/$OLD_NAME/$NEW_NAME/g")
    echo "Renaming folder: $dir -> $newdir"
    mv "$dir" "$newdir"
done

# Step 2: Ganti nama file
find . -type f -name "*$OLD_NAME*" | while read file; do
    newfile=$(echo "$file" | sed "s/$OLD_NAME/$NEW_NAME/g")
    echo "Renaming file: $file -> $newfile"
    mv "$file" "$newfile"
done

# Step 3: Replace isi file (namespace, project name, dll)
echo "Replacing content inside files..."
grep -rl "$OLD_NAME" . | xargs sed -i "s/$OLD_NAME/$NEW_NAME/g"

# Step 4: Rename solution file jika ada
if [ -f "$OLD_NAME.sln" ]; then
    echo "Renaming solution file: $OLD_NAME.sln -> $NEW_NAME.sln"
    mv "$OLD_NAME.sln" "$NEW_NAME.sln"
fi

echo "✅ Proses rename selesai. Jangan lupa cek namespace/using/import manual kalau ada yang nyelip."

