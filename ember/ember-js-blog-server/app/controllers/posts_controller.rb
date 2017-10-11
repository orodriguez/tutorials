class PostsController < ApplicationController
  def index
    render :json => { posts: Post.all }, :except => [:created_at, :updated_at]
  end

  def update
    post = Post.find params[:id]
    return render :json => 'fail' unless post.update_attributes(params[:post])
    render :json => 'success'
  end
end
